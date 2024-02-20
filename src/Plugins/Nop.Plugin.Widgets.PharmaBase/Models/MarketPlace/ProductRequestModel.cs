using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace
{
    public class ProductRequestModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public int CargoCompanyId { get; set; }

        public string StockCode { get; set; }

        public string Barcode { get; set; }

        public int DimensionalWeight { get; set; }

        public string ProductMainId { get; set; }

        public string TargetMarketPlaceType { get; set; }

        public string CurrencyType { get; set; }
        public decimal ListPrice { get; set; }

        public decimal SalePrice { get; set; }
        public int Vat { get; set; }

        public List<ProductAttributes> Attributes { get; set; }

        public List<ProductImage> Images { get; set; }

    }

    public class ProductImage
    {
        public string Url { get; set; }
    }

    public class ProductAttributes
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public int? AttributeValueId { get; set; }
        public string AttributeValue { get; set; }
    }
}
