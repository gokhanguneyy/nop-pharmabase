using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.PharmaBase
{
    public class PharmaBaseSettings : ISettings
    {
        public string Url { get; set; }

        #region trendyol
        public string trendyolDeveloperUserName { get; set; }
        public string trendyolPassword { get; set; }
        public string trendyolSupplierId { get; set; }
        #endregion

        #region hepsiburada
        public string hepsiburadaDeveloperUserName { get; set; }
        public string hepsiburadaPassword { get; set; }
        public string hepsiburadaSupplierId { get; set; }
        #endregion

        #region amazon
        public string amazonDeveloperUserName { get; set; }
        public string amazonPassword { get; set; }
        public string amazonSupplierId { get; set; }
        #endregion
    
        public int NopCategoryId { get; set; }
        public int TrendyolCategoryId { get; set;}
        public int NopManufacturerId { get; set; }  
        public int TrendyolBrandId { get; set; }
    }
}