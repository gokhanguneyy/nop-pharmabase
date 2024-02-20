using System.Runtime.InteropServices;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.Widgets.PharmaBase.Models;
using Nop.Plugin.Widgets.PharmaBase.Models.Attributes;
using Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace;
using Nop.Plugin.Widgets.PharmaBase.Service;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Plugin.Widgets.PharmaBase.Controller
{
    public class WidgetsPharmaBaseController : BaseAdminController
    {

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IMarketPlaceCategoryServiceInterface _marketPlaceCategoryService;
        private readonly IProductService _productService;
        private readonly IProductAttributeModelFactory _productAttributeModelFactory;
        private readonly IProductAttributeService _productAttributeService;
        public WidgetsPharmaBaseController(ILocalizationService localizationService,
        IProductService productService,
        INotificationService notificationService,
        IPermissionService permissionService,
        ISettingService settingService,
        IStoreContext storeContext,
        IBaseAdminModelFactory baseAdminModelFactory,
        IMarketPlaceCategoryServiceInterface marketPlaceCategoryService,
        IProductAttributeModelFactory productAttributeModelFactory,
        IProductAttributeService productAttributeService)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _baseAdminModelFactory = baseAdminModelFactory;
            _marketPlaceCategoryService = marketPlaceCategoryService;
            _productService = productService;
            _productAttributeModelFactory = productAttributeModelFactory;
            _productAttributeService = productAttributeService;
        }

        public async Task<int> GetProducts()
        {
            var products = await _productService.SearchProductsAsync();
            var productCount = products.Count();
            return productCount;
        }

        public async Task<IActionResult> Configure()
        {

            var productCount = await GetProducts();

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var pharmaBaseSettings = await _settingService.LoadSettingAsync<PharmaBaseSettings>(storeScope);

            var model = new ConfigurationModel()
            {
                ActiveStoreScopeConfiguration = storeScope,

                Urls = "https://localhost:44309/",

                message = productCount.ToString() + " ürün mevcuttur.",


                trendyolDeveloperUserNames = pharmaBaseSettings.trendyolDeveloperUserName,
                trendyolPasswords = pharmaBaseSettings.trendyolPassword,
                trendyolSupplierIds = pharmaBaseSettings.trendyolSupplierId,

                hepsiburadaDeveloperUserNames = pharmaBaseSettings.hepsiburadaDeveloperUserName,
                hepsiburadaPasswords = pharmaBaseSettings.hepsiburadaPassword,
                hepsiburadaSupplierIds = pharmaBaseSettings.hepsiburadaSupplierId,

                amazonDeveloperUserNames = pharmaBaseSettings.amazonDeveloperUserName,
                amazonPasswords = pharmaBaseSettings.amazonPassword,
                amazonSupplierIds = pharmaBaseSettings.amazonSupplierId,

            };

            if (storeScope > 0)
            {

                model.Urls_OverrideForStore = await _settingService.SettingExistsAsync(pharmaBaseSettings, x => x.Url, storeScope);

                model.trendyolDeveloperUserNames_OverrideForStore = await _settingService.SettingExistsAsync(pharmaBaseSettings, x => x.trendyolDeveloperUserName, storeScope);
                model.trendyolPasswords_OverrideForStroe = await _settingService.SettingExistsAsync(pharmaBaseSettings, x => x.trendyolPassword, storeScope);
                model.trendyolSupplierIds_OverrideForStore = await _settingService.SettingExistsAsync(pharmaBaseSettings, x => x.trendyolSupplierId, storeScope);

                model.hepsiburadaDeveloperUserNames_OverrideForStore = await _settingService.SettingExistsAsync(pharmaBaseSettings, x => x.hepsiburadaDeveloperUserName, storeScope);
                model.hepsiburadaPasswords_OverrideForStroe = await _settingService.SettingExistsAsync(pharmaBaseSettings, x => x.hepsiburadaPassword, storeScope);
                model.hepsiburadaSupplierIds_OverrideForStore = await _settingService.SettingExistsAsync(pharmaBaseSettings, x => x.hepsiburadaSupplierId, storeScope);

                model.amazonDeveloperUserNames_OverrideForStore = await _settingService.SettingExistsAsync(pharmaBaseSettings, x => x.amazonDeveloperUserName, storeScope);
                model.amazonPasswords_OverrideForStroe = await _settingService.SettingExistsAsync(pharmaBaseSettings, x => x.amazonPassword, storeScope);
                model.amazonSupplierIds_OverrideForStore = await _settingService.SettingExistsAsync(pharmaBaseSettings, x => x.amazonSupplierId, storeScope);


            }

            //NOP DEFAULT CATEGORIES
             await _baseAdminModelFactory.PrepareCategoriesAsync(model.AvailableCategories,
             defaultItemText: await _localizationService.GetResourceAsync("Admin.Catalog.Categories.Fields.Parent.None"));

            //NOP DEFAULT MANUFACTURERS
            await _baseAdminModelFactory.PrepareManufacturersAsync(model.AvailableManufacturers, false);


            PagingResponseModel<List<MarketPlaceCategoriesResponse>> categories = new PagingResponseModel<List<MarketPlaceCategoriesResponse>>();

            categories = await _marketPlaceCategoryService.GetCategoriesAsync();

            var trendyolBrands = new PagingResponseModel<List<BrandResponse>>();

            trendyolBrands = await _marketPlaceCategoryService.GetBrandsAsync();

            //foreach brand icin select list item olustur
            List<SelectListItem> brandNamesAndIds = new List<SelectListItem>();


            foreach (var brand in trendyolBrands.Data)
            {
                foreach (var item in brand.Brands)
                {
                    var currentBrand = new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    };
                    brandNamesAndIds.Add(currentBrand);
                }
            }

            model.AvailableTrendyolBrands = brandNamesAndIds;

            //ExtractCategoryNamesAndIds
            List<SelectListItem> namesAndIds = new List<SelectListItem>();
            namesAndIds = ExtractCategoryNamesAndIds(categories.Data);

            //
            model.AvailableTrendyolCategories = namesAndIds;


            //ALL PRODUCT ATTRIBUTES
            var listOfProductAttributes = await _productAttributeModelFactory.PrepareProductAttributeListModelAsync(new ProductAttributeSearchModel());
            List<SelectListItem> productAtrributes = new List<SelectListItem>();
            foreach (var productAttribute in listOfProductAttributes.Data)
            {
                var currentProductAttribute = new SelectListItem()
                {
                    Text = productAttribute.Name,
                    Value = productAttribute.Id.ToString()
                };
                productAtrributes.Add(currentProductAttribute);
            }

            model.AvailableProductAttributes = productAtrributes;

            return View("~/Plugins/Widgets.PharmaBase/Views/Configure.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributes(int categoryId)
        {
            var attributesOfCategory = new List<SelectListItem>();

            var test =await _marketPlaceCategoryService.GetAttributesByCategoryId(categoryId);

            //fill select list items
            foreach (var attribute in test.Data)
            {
                foreach (var catAttribute in attribute.CategoryAttributes)
                {
                    var currentAttribute = new SelectListItem()
                    {
                        Text = catAttribute.Attribute.Name,
                        Value = catAttribute.Attribute.Id.ToString()
                    };
                    attributesOfCategory.Add(currentAttribute);
                }
            }
            return Ok(attributesOfCategory);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributeValues(int categoryId,int attributeId)
        {
            var valuesOfCategory = new List<SelectListItem>();

            var test = await _marketPlaceCategoryService.GetAttributesByCategoryId(categoryId);
            var categoryAttributeValuesList=new List<TAttributeValue>();

            var found = false;
            foreach (var attribute in test.Data)
            {
                foreach (var catAttribute in attribute.CategoryAttributes)
                {

                    if (catAttribute.Attribute.Id== attributeId)
                    {
                        
                       categoryAttributeValuesList= catAttribute.AttributeValues;
                       found = true;
                    }
                    else if (found)
                    {
                        break;
                    }

                }
            }

            foreach (var catAttributeValue in categoryAttributeValuesList)
            {
                var currentAttributeValue = new SelectListItem()
                {
                    Text = catAttributeValue.Name,
                    Value = catAttributeValue.Id.ToString()
                };
                valuesOfCategory.Add(currentAttributeValue);

            }

            return Ok(valuesOfCategory);
        }

        [HttpGet]
        public async Task<IActionResult> GetNopProductAttributeValues(int nopAttributeId)
        {
            var valuesOfProductAttribute = new List<SelectListItem>();

            var productAttribute = await _productAttributeService.GetProductAttributeByIdAsync(nopAttributeId);
            PredefinedProductAttributeValueSearchModel searchModel = new PredefinedProductAttributeValueSearchModel();
            searchModel.ProductAttributeId = nopAttributeId;


            //prepare model
            var model = await _productAttributeModelFactory.PreparePredefinedProductAttributeValueListModelAsync(searchModel, productAttribute);

            foreach (var item in model.Data)
            {
                var currentBrand = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                };
                valuesOfProductAttribute.Add(currentBrand);

            }



            return Ok(valuesOfProductAttribute);
        }
        

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();


            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var pharmaBaseSettings = await _settingService.LoadSettingAsync<PharmaBaseSettings>(storeScope);

            pharmaBaseSettings.Url = model.Urls;

            pharmaBaseSettings.trendyolDeveloperUserName = model.trendyolDeveloperUserNames;
            pharmaBaseSettings.trendyolPassword = model.trendyolPasswords;
            pharmaBaseSettings.trendyolSupplierId = model.trendyolSupplierIds;

            pharmaBaseSettings.hepsiburadaDeveloperUserName = model.hepsiburadaDeveloperUserNames;
            pharmaBaseSettings.hepsiburadaPassword = model.hepsiburadaPasswords;
            pharmaBaseSettings.hepsiburadaSupplierId = model.hepsiburadaSupplierIds;

            pharmaBaseSettings.amazonDeveloperUserName = model.amazonDeveloperUserNames;
            pharmaBaseSettings.amazonPassword = model.amazonPasswords;
            pharmaBaseSettings.amazonSupplierId = model.amazonSupplierIds;

            pharmaBaseSettings.NopCategoryId = model.ParentCategoryId;
            pharmaBaseSettings.NopManufacturerId = model.BrandId;
            pharmaBaseSettings.TrendyolBrandId = model.TrendYolBrandId;
            pharmaBaseSettings.TrendyolCategoryId = model.TrendYolCategoryId;


            await _settingService.SaveSettingOverridablePerStoreAsync(pharmaBaseSettings, x => x.Url, model.Urls_OverrideForStore, storeScope, false);

            await _settingService.SaveSettingOverridablePerStoreAsync(pharmaBaseSettings, x => x.trendyolDeveloperUserName, model.trendyolDeveloperUserNames_OverrideForStore, storeScope, true);
            await _settingService.SaveSettingOverridablePerStoreAsync(pharmaBaseSettings, x => x.trendyolPassword, model.trendyolPasswords_OverrideForStroe, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(pharmaBaseSettings, x => x.trendyolSupplierId, model.trendyolSupplierIds_OverrideForStore, storeScope, false);

            await _settingService.SaveSettingOverridablePerStoreAsync(pharmaBaseSettings, x => x.hepsiburadaDeveloperUserName, model.hepsiburadaDeveloperUserNames_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(pharmaBaseSettings, x => x.hepsiburadaPassword, model.hepsiburadaPasswords_OverrideForStroe, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(pharmaBaseSettings, x => x.hepsiburadaSupplierId, model.hepsiburadaSupplierIds_OverrideForStore, storeScope, false);

            await _settingService.SaveSettingOverridablePerStoreAsync(pharmaBaseSettings, x => x.amazonDeveloperUserName, model.amazonDeveloperUserNames_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(pharmaBaseSettings, x => x.amazonPassword, model.amazonPasswords_OverrideForStroe, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(pharmaBaseSettings, x => x.amazonSupplierId, model.amazonSupplierIds_OverrideForStore, storeScope, false);


            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));


            return await Configure();
        }

        public List<SelectListItem> ExtractCategoryNamesAndIds(List<MarketPlaceCategoriesResponse> categoriesResponses)
        {
            List<SelectListItem> categoryNamesAndIds = new List<SelectListItem>();

            foreach (var categoriesResponse in categoriesResponses)
            {
                foreach (var categoryData in categoriesResponse.Data)
                {
                    categoryNamesAndIds.AddRange(ExtractNamesAndIdsRecursive(categoryData, ""));
                }
            }

            return categoryNamesAndIds;
        }

        private List<SelectListItem> ExtractNamesAndIdsRecursive(CategoryData categoryData, string parentNames)
        {
            List<SelectListItem> namesAndIds = new List<SelectListItem>();

            string categoryName = parentNames == "" ? categoryData.Name : $"{parentNames} > {categoryData.Name}";
            namesAndIds.Add(new SelectListItem { Value = categoryData.CategoryId.ToString(), Text = categoryName });

            string currentParentNames = parentNames == "" ? categoryData.Name : $"{parentNames} > {categoryData.Name}";

            foreach (var subCategoryData in categoryData.SubCategoryData)
            {
                namesAndIds.AddRange(ExtractNamesAndIdsRecursive(subCategoryData, currentParentNames));
            }

            return namesAndIds;
        }

    }
}