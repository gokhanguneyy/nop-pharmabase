using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Services.Localization;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.PharmaBase.Models
{
    public record class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        public string Urls { get; set; }

        public bool Urls_OverrideForStore { get; set; }

        public string message { get; set; }

        #region trendyol

        public string trendyolDeveloperUserNames { get; set; }
        public bool trendyolDeveloperUserNames_OverrideForStore { get; set; }
        public string trendyolPasswords { get; set; }
        public bool trendyolPasswords_OverrideForStroe { get; set; }
        public string trendyolSupplierIds { get; set; }
        public bool trendyolSupplierIds_OverrideForStore { get; set; }

        #endregion

        #region hepsiburada

        public string hepsiburadaDeveloperUserNames { get; set; }
        public bool hepsiburadaDeveloperUserNames_OverrideForStore { get; set; }
        public string hepsiburadaPasswords { get; set; }
        public bool hepsiburadaPasswords_OverrideForStroe { get; set; }
        public string hepsiburadaSupplierIds { get; set; }
        public bool hepsiburadaSupplierIds_OverrideForStore { get; set; }

        #endregion

        #region amazon

        public string amazonDeveloperUserNames { get; set; }
        public bool amazonDeveloperUserNames_OverrideForStore { get; set; }
        public string amazonPasswords { get; set; }
        public bool amazonPasswords_OverrideForStroe { get; set; }
        public string amazonSupplierIds { get; set; }
        public bool amazonSupplierIds_OverrideForStore { get; set; }


        #endregion

        #region NopDefaultCategoriesAndManufacturers
        [NopResourceDisplayName("Admin.Catalog.Categories.Fields.Parent")]
        public int ParentCategoryId { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }

        public int TrendYolCategoryId { get; set; }
        public IList<SelectListItem> AvailableTrendyolCategories { get; set; }

        public int TrendYolBrandId { get; set; }
        public IList<SelectListItem> AvailableTrendyolBrands { get; set; }

        public int BrandId { get; set; }

        public IList<SelectListItem> AvailableManufacturers { get; set; }

        public int ProductAttributeId { get; set; }

        public IList<SelectListItem> AvailableProductAttributes { get; set; }



        #endregion
        public ConfigurationModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableTrendyolCategories = new List<SelectListItem>();
            AvailableTrendyolBrands = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
            AvailableProductAttributes = new List<SelectListItem>();

        }

    }
}