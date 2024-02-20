using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace
{
    public class MarketPlaceCategoriesResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public int Version { get; set; }
        public string Message { get; set; }
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
        public int Number { get; set; }
        public int NumberOfElements { get; set; }
        public bool First { get; set; }
        public bool Last { get; set; }
        public MarketPlaceType MarketPlaceType { get; set; }
        public IList<CategoryData> Data { get; set; }

        public MarketPlaceCategoriesResponse()
        {
            Data = new List<CategoryData>();
        }
    }

    public class CategoryData
    {
        public CategoryData()
        {
            SubCategoryData = new List<CategoryData>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public object ParentId { get; set; }
        public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public string[] Paths { get; set; }
        public bool Leaf { get; set; }
        public string Status { get; set; }
        public bool Available { get; set; }
        public IList<CategoryData> SubCategoryData { get; set; }

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MarketPlaceType : short
    {
        Trendyol = 1,
        Hepsiburada = 2,
        N11 = 3,
        Amazon = 4
    }
}
