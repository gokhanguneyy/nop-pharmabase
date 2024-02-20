using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace;
using Nop.Plugin.Widgets.PharmaBase.Service.Auth;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.PharmaBase.Controller.MarketPlaceEndPoints.Products
{
    public class SendProductController : BasePluginController
    {
        private readonly IProductService _productService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IPictureService _pictureService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICategoryService _categoryService;
        private readonly IWorkContext _workContext;
        private readonly HttpClient _httpClient;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;


        public SendProductController(
            IProductService productService,
            IStoreContext storeContext,
            ISettingService settingService,
            IProductAttributeService productAttributeService,
            IPictureService pictureService,
            IWorkContext workContext,
            IManufacturerService manufacturerService,
            ICategoryService categoryService,
            HttpClient httpClient
            )
        {
            _productService = productService;
            _productAttributeService = productAttributeService;
            _pictureService = pictureService;
            _manufacturerService = manufacturerService;
            _workContext = workContext;
            _categoryService = categoryService;
            _httpClient = httpClient;
            _storeContext = storeContext;
            _settingService = settingService;
        }

        string _result;

        [HttpPost]
        public async Task<IActionResult> SendProducts()
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var pharmaBaseSettings = await _settingService.LoadSettingAsync<PharmaBaseSettings>(store.Id);
            var baseUrl = pharmaBaseSettings.Url;

            try
            {
                var productsModels = await GetInformation();


                var authService = new BasicAuthentication(_httpClient);
                authService.Authenticate();

                var url = $"{baseUrl}api/v1/products";

                //var url = "https://localhost:44309/api/v1/products";

                foreach (var products in productsModels)
                {
                    var json = JsonConvert.SerializeObject(products);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync(url, data);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                        _result = content;
                    else
                        return StatusCode((int)response.StatusCode, content);
                }
                return Ok(false);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }




        public async Task<List<ProductRequestModel>> GetInformation()
        {
            var products = await _productService.SearchProductsAsync();
            var images = new List<ProductImage>();
            var attributes = new List<ProductAttributes>();

            var productsModels = new List<ProductRequestModel>();

            foreach (var p in products)
            {
                var brandId = await GetBrandId(p.Id);
                var category = await GetCategoryId(p.Id);
                var currency = await GetCurrencyCode();
                var picture = new ProductImage
                {
                    Url = await GetImageUrl(p.Id),
                };
                images.Add(picture);

                var attribute = await GetAttributesInfo(p.Id);
                attributes.Add(attribute);

                var productModel = new ProductRequestModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Description = p.FullDescription,
                    BrandId = brandId,
                    CategoryId = category,
                    Quantity = p.StockQuantity,
                    CargoCompanyId = 1,
                    StockCode = p.Sku,
                    Barcode = p.Gtin,
                    DimensionalWeight = Convert.ToInt32(p.Height * p.Width * p.Length / 5000),
                    ProductMainId = p.ParentGroupedProductId.ToString(),
                    CurrencyType = currency,
                    SalePrice = p.Price,
                    ListPrice = p.OldPrice,
                    Vat = 20,
                    Images = images,
                    Attributes = attributes,
                };
                productsModels.Add(productModel);
            }

            return productsModels;

        }

        #region GET INFO OF PRODUCTS FROM NOP SERVICES
        public async Task<string> GetCurrencyCode()
        {
            var currencies = await _workContext.GetWorkingCurrencyAsync();

            string currencyCode = currencies.CurrencyCode;

            return currencyCode;
        }

        public async Task<int> GetBrandId(int productId)
        {
            var brands = await _manufacturerService.GetProductManufacturersByProductIdAsync(productId);
            var brandId = brands.Select(x => x.ManufacturerId).FirstOrDefault();
            return brandId;
        }

        public async Task<int> GetCategoryId(int productId)
        {
            var categories = await _categoryService.GetProductCategoriesByProductIdAsync(productId);
            var categoryId = categories.Select(x => x.CategoryId).FirstOrDefault();
            return categoryId;
        }

        public async Task<string> GetImageUrl(int productId)
        {
            var products = await _productService.GetProductPicturesByProductIdAsync(productId);
            var pictureId = products.Select(x => x.PictureId).FirstOrDefault();
            var pictures = await _pictureService.GetPictureUrlAsync(pictureId);
            return pictures;
        }

        public async Task<ProductAttributes> GetAttributesInfo(int productId)
        {
            var productAttributes = await _productAttributeService.GetProductAttributeMappingsByProductIdAsync(productId);
            var productAttributeId = productAttributes.Select(x => x.ProductAttributeId).FirstOrDefault();
            var productAttributeValueId = productAttributes.Select(x => x.Id).FirstOrDefault();



            string productAttributeName;
            if (productAttributeId > 0)
            {
                var productAttributeNames = await _productAttributeService.GetProductAttributeByIdAsync(productAttributeId);
                productAttributeName = productAttributeNames.Name;
            }
            else
            {
                productAttributeName = string.Empty;
            }

            string productAttributeValueName;
            if (productAttributeValueId > 0)
            {
                var productAttributeValueNames = await _productAttributeService.GetProductAttributeValueByIdAsync(productAttributeValueId);
                productAttributeValueName = productAttributeValueNames.Name;
            }
            else
            {
                productAttributeValueName = string.Empty;
            }

            var model = new ProductAttributes
            {
                AttributeId = productAttributeId,
                AttributeName = productAttributeName,
                AttributeValueId = productAttributeValueId,
                AttributeValue = productAttributeValueName,
            };

            return model;
        }
        #endregion
    }


}