using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Events;
using Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace;
using Nop.Plugin.Widgets.PharmaBase.Service.Auth;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Events;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.PharmaBase.Consumer
{
    public class ProductUpdatedConsumer : IConsumer<EntityUpdatedEvent<Product>>
    {
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly IProductService _productService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IPictureService _pictureService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICategoryService _categoryService;
        private readonly IWorkContext _workContext;

        public ProductUpdatedConsumer(
            IStoreContext storeContext,
            ISettingService settingService,
            IProductService productService,
            IProductAttributeService productAttributeService,
            IPictureService pictureService,
            IManufacturerService manufacturerService,
            ICategoryService categoryService,
            IWorkContext workContext)
        {
            _storeContext = storeContext;
            _settingService = settingService;
            _productService = productService;
            _productAttributeService = productAttributeService;
            _pictureService = pictureService;
            _manufacturerService = manufacturerService;
            _categoryService = categoryService;
            _workContext = workContext;
        }

        public async Task HandleEventAsync(EntityUpdatedEvent<Product> eventMessage)
        {
            var product = eventMessage.Entity;
            await UpdateProductInExternalApi(product);
        }

        private async Task<IActionResult> UpdateProductInExternalApi(Product product)
        {

            var store = await _storeContext.GetCurrentStoreAsync();
            var pharmaBaseSettings = await _settingService.LoadSettingAsync<PharmaBaseSettings>(store.Id);
            string baseUrl = pharmaBaseSettings.Url;

            var products = await GetInformation(product);

            var client = new HttpClient();
            var url = $"{baseUrl}api/v1/products";
            //var url = "https://localhost:44309/api/v1/products";

            var authService = new BasicAuthentication(client);
            authService.Authenticate();

            var content = new StringContent(JsonConvert.SerializeObject(products), Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new OkObjectResult(responseContent);
            }
            else
            {
                return new ObjectResult(responseContent)
                {
                    StatusCode = (int)response.StatusCode
                };
            }
        }

        public async Task<ProductRequestModel> GetInformation(Product newProduct)
        {
            var brandId = await GetBrandId(newProduct.Id);
            var category = await GetCategoryId(newProduct.Id);
            var currency = await GetCurrencyCode();
            var images = new List<ProductImage>();
            var picture = new ProductImage
            {
                Url = await GetImageUrl(newProduct.Id),
            };
            images.Add(picture);
            var attributes = new List<ProductAttributes>();
            var attribute = await GetAttributesInfo(newProduct.Id);
            attributes.Add(attribute);

            var productModel = new ProductRequestModel
            {
                Id = newProduct.Id.ToString(),
                Name = newProduct.Name,
                Description = newProduct.FullDescription,
                BrandId = brandId,
                CategoryId = category,
                Quantity = newProduct.StockQuantity,
                CargoCompanyId = 1,
                StockCode = newProduct.Sku,
                Barcode = newProduct.Gtin,
                DimensionalWeight = Convert.ToInt32(newProduct.Height * newProduct.Width * newProduct.Length / 5000),
                ProductMainId = newProduct.ParentGroupedProductId.ToString(),
                CurrencyType = currency,
                SalePrice = newProduct.Price,
                ListPrice = newProduct.OldPrice,
                Vat = 20,
                Images = images,
                Attributes = attributes,


            };

            return productModel;
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


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using Nop.Core;
//using Nop.Core.Domain.Catalog;
//using Nop.Core.Events;
//using Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace;
//using Nop.Plugin.Widgets.PharmaBase.Service.Auth;
//using Nop.Services.Catalog;
//using Nop.Services.Events;
//using Nop.Services.Media;

//namespace Nop.Plugin.Widgets.PharmaBase.Consumer
//{
//    public class ProductUpdatedConsumer : IConsumer<EntityUpdatedEvent<Product>>
//    {
//        private readonly IProductService _productService;
//        private readonly IProductAttributeService _productAttributeService;
//        private readonly IPictureService _pictureService;
//        private readonly IManufacturerService _manufacturerService;
//        private readonly ICategoryService _categoryService;
//        private readonly IWorkContext _workContext;

//        public ProductUpdatedConsumer(
//            IProductService productService,
//            IProductAttributeService productAttributeService,
//            IPictureService pictureService,
//            IManufacturerService manufacturerService,
//            ICategoryService categoryService,
//            IWorkContext workContext)
//        {
//            _productService = productService;
//            _productAttributeService = productAttributeService;
//            _pictureService = pictureService;
//            _manufacturerService = manufacturerService;
//            _categoryService = categoryService;
//            _workContext = workContext;
//        }

//        public async Task HandleEventAsync(EntityUpdatedEvent<Product> eventMessage)
//        {
//            var product = eventMessage.Entity;
//            await UpdateProductInExternalApi(product);
//        }

//        private async Task<IActionResult> UpdateProductInExternalApi(Product product)
//        {
//            var products = await GetInformation(product);

//            var client = new HttpClient();
//            var url = "https://localhost:44309/api/v1/products";

//            var authService = new BasicAuthentication(client);
//            authService.Authenticate();

//            var content = new StringContent(JsonConvert.SerializeObject(products), Encoding.UTF8, "application/json");

//            var response = await client.PutAsync(url, content);

//            var responseContent = await response.Content.ReadAsStringAsync();

//            if (response.IsSuccessStatusCode)
//            {
//                return new OkObjectResult(responseContent);
//            }
//            else
//            {
//                return new ObjectResult(responseContent)
//                {
//                    StatusCode = (int)response.StatusCode
//                };
//            }
//        }

//        public async Task<ProductRequestModel> GetInformation(Product newProduct)
//        {
//            var brandId = await GetBrandId(newProduct.Id);
//            var category = await GetCategoryId(newProduct.Id);
//            var currency = await GetCurrencyCode();
//            var images = new List<ProductImage>();
//            var picture = new ProductImage
//            {
//                Url = await GetImageUrl(newProduct.Id),
//            };
//            images.Add(picture);
//            var attributes = new List<ProductAttributes>();
//            var attribute = await GetAttributesInfo(newProduct.Id);
//            attributes.Add(attribute);

//            var productModel = new ProductRequestModel
//            {
//                Id = newProduct.Id.ToString(),
//                Name = newProduct.Name,
//                Description = newProduct.FullDescription,
//                BrandId = brandId,
//                CategoryId = category,
//                Quantity = newProduct.StockQuantity,
//                CargoCompanyId = 1,
//                StockCode = newProduct.Sku,
//                Barcode = newProduct.Gtin,
//                DimensionalWeight = Convert.ToInt32(newProduct.Height * newProduct.Width * newProduct.Length / 5000),
//                ProductMainId = newProduct.ParentGroupedProductId.ToString(),
//                CurrencyType = currency,
//                SalePrice = newProduct.Price,
//                ListPrice = newProduct.OldPrice,
//                Vat = 20,
//                Images = images,
//                Attributes = attributes,


//            };

//            return productModel;
//        }

//        #region GET INFO OF PRODUCTS FROM NOP SERVICES
//        public async Task<string> GetCurrencyCode()
//        {
//            var currencies = await _workContext.GetWorkingCurrencyAsync();

//            string currencyCode = currencies.CurrencyCode;

//            return currencyCode;
//        }

//        public async Task<int> GetBrandId(int productId)
//        {
//            var brands = await _manufacturerService.GetProductManufacturersByProductIdAsync(productId);
//            var brandId = brands.Select(x => x.ManufacturerId).FirstOrDefault();
//            return brandId;
//        }

//        public async Task<int> GetCategoryId(int productId)
//        {
//            var categories = await _categoryService.GetProductCategoriesByProductIdAsync(productId);
//            var categoryId = categories.Select(x => x.CategoryId).FirstOrDefault();
//            return categoryId;
//        }

//        public async Task<string> GetImageUrl(int productId)
//        {
//            var products = await _productService.GetProductPicturesByProductIdAsync(productId);
//            var pictureId = products.Select(x => x.PictureId).FirstOrDefault();
//            var pictures = await _pictureService.GetPictureUrlAsync(pictureId);
//            return pictures;
//        }

//        public async Task<ProductAttributes> GetAttributesInfo(int productId)
//        {
//            var productAttributes = await _productAttributeService.GetProductAttributeMappingsByProductIdAsync(productId);
//            var productAttributeId = productAttributes.Select(x => x.ProductAttributeId).FirstOrDefault();
//            var productAttributeValueId = productAttributes.Select(x => x.Id).FirstOrDefault();



//            string productAttributeName;
//            if (productAttributeId > 0)
//            {
//                var productAttributeNames = await _productAttributeService.GetProductAttributeByIdAsync(productAttributeId);
//                productAttributeName = productAttributeNames.Name;
//            }
//            else
//            {
//                productAttributeName = string.Empty;
//            }

//            string productAttributeValueName;
//            if (productAttributeValueId > 0)
//            {
//                var productAttributeValueNames = await _productAttributeService.GetProductAttributeValueByIdAsync(productAttributeValueId);
//                productAttributeValueName = productAttributeValueNames.Name;
//            }
//            else
//            {
//                productAttributeValueName = string.Empty;
//            }

//            var model = new ProductAttributes
//            {
//                AttributeId = productAttributeId,
//                AttributeName = productAttributeName,
//                AttributeValueId = productAttributeValueId,
//                AttributeValue = productAttributeValueName,
//            };

//            return model;
//        }
//        #endregion
//    }
//}
