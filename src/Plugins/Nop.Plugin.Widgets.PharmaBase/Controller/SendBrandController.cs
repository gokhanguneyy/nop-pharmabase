using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Plugin.Widgets.PharmaBase.Models;
using Nop.Services.Configuration;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.PharmaBase.Controller
{
    public class SendBrandController : BasePluginController
    {
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;

        public SendBrandController(ISettingService settingService, IStoreContext storeContext)
        {
            _settingService = settingService;
            _storeContext = storeContext;
        }

        [HttpPost]
        public async Task SendBr(MarketPlaceBrandRequest request)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var pharmaBaseSettings = await _settingService.LoadSettingAsync<PharmaBaseSettings>(store.Id);
            string baseUrl = pharmaBaseSettings.Url;

            var httpClient = new HttpClient();
            // API endpoint URL'i
            string url = $"{baseUrl}api/v1/brands";

            // JSON içeriğimiz

            var requestData = new
            {
                brandId = request.BrandId,
                trendyolBrandId = request.TrendyolBrandId,
                hepsiburadaBrandId = 0,
                n11BrandId = 0,
                amazonBrandId = 0
            };

            var json = JsonConvert.SerializeObject(requestData);
            // request.Content = new StringContent(json, Encoding.UTF8, "application/json");


            // İstek için gerekli header'lar ekleniyor
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Basic Auth için
            //var auth = new BasicAuthentication(httpClient);
            //auth.Authenticate();
            var byteArray = Encoding.ASCII.GetBytes("demo_admin:demo_password");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // HTTP POST isteği gönderiliyor
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
