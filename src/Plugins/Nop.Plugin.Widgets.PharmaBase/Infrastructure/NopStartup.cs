using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Domain.Catalog;
using Nop.Core.Events;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.PharmaBase.Consumer;
using Nop.Plugin.Widgets.PharmaBase.Helper;
using Nop.Plugin.Widgets.PharmaBase.Service;
using Nop.Plugin.Widgets.PharmaBase.Service.MarketPlace;
using Nop.Services.Events;

namespace Nop.Plugin.Widgets.PharmaBase.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public int Order => 1;

        public void Configure(IApplicationBuilder application)
        {
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IConsumer<EntityInsertedEvent<Product>>, ProductAddedConsumer>();
            services.AddScoped<IConsumer<EntityUpdatedEvent<Product>>, ProductUpdatedConsumer>();
            services.AddScoped<IMarketPlaceCategoryServiceInterface, MarketPlaceCategoryService>();
            services.AddScoped<ISerializerHelper, JsonSerializerHelper>();

        }
    }
}
