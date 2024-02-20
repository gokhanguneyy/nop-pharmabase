using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.PharmaBase.Components
{
    public class PharmaBaseViewComponent : NopViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {

           return View("~/Plugins/Widgets.PharmaBase/Views/PharmaBase.cshtml");
        }
    }
}
