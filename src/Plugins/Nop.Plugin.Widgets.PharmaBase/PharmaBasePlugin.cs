using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.PharmaBase
{
    public class PharmaBasePlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService; 
        private readonly ISettingService _settingService; 

        public PharmaBasePlugin(IWebHelper webHelper, ILocalizationService localizationService, ISettingService settingService)
        {
            _webHelper = webHelper;
            _localizationService = localizationService;
            _settingService = settingService;
        }

        // WIDGET LİSTESİNDE, WIDGET'IN GÖRÜNÜP GÖRÜNMEYECEĞİNİ BELİRLERDĞİMİZ METHOD
        public bool HideInWidgetList => false;


        // HANGİ WIDGET'IN GÖSTERİLECEĞİNİ BELİRLEDİĞİMİZ METHOD
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "PharmaBase";
        }


        // WIDGET'IN SİTE İÇERİSİNDE NEREDE GÖSTERİLECEĞİNİ BELİRLEDİĞİMİZ METHOD
        public async Task<IList<string>> GetWidgetZonesAsync()
        {
            var widgetZones = new List<string> { PublicWidgetZones.ProductDetailsInsideOverviewButtonsAfter };

            return await Task.FromResult(widgetZones);
        }


        // CONFIGURE PAGE
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsPharmaBase/Configure";
        }
        public override async Task InstallAsync()
        {
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.PharmaBase.Trendyol"] = "TRENDYOL",
                ["Plugins.Widgets.PharmaBase.Hepisburada"] = "HEPSİBURADA",
                ["Plugins.Widgets.PharmaBase.Amazon"] = "AMAZON",
                ["Plugins.Widgets.Pharmabase.Ürünlerigönder"] = "ÜRÜN GÖNDER",
            });

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await _localizationService.DeleteLocaleResourceAsync("Plugins.Widgets.PharmaBase");

            await base.UninstallAsync();
        }
    }
}
