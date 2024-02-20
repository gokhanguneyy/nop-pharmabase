using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace;
using Nop.Services.Catalog;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.PharmaBase.Service.ProductsInfo
{
    public class GetInfo
    {
        IProductService _productService;
        IProductAttributeService _productAttributeService;
        IPictureService _pictureService;
        IManufacturerService _manufacturerService;
        ICategoryService _categoryService;
        IWorkContext _workContext;

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
