using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.GetOrders
{
    public class GetOrdersPlugin : BasePlugin,IWidgetPlugin
    {
        public bool HideInWidgetList => false;

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "OrdersList";
        }

        public async Task<IList<string>> GetWidgetZonesAsync()
        {
            var widgetZones = new List<string> { AdminWidgetZones.OrderDetailsButtons};

            return await Task.FromResult(widgetZones);
        }

        public override Task InstallAsync()
        {
            return base.InstallAsync();
        }
        public override Task UninstallAsync() {
            return base.UninstallAsync();
        }
    }
}
