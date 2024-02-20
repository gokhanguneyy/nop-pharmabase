using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Plugin.Widgets.PharmaBase.Models;
using Nop.Services.Configuration;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.PharmaBase.Controller
{
    public class SendCategoryController: BasePluginController
    {
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;

        public SendCategoryController(IStoreContext storeContext, ISettingService settingService)
        {
            _storeContext = storeContext;
            _settingService = settingService;
        }

        [HttpPost]
        public async Task SendCt(MarketPlaceCategoryRequest request)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var pharmaBaseSettings = await _settingService.LoadSettingAsync<PharmaBaseSettings>(store.Id);
            string baseUrl = pharmaBaseSettings.Url;

            var httpClient = new HttpClient();
            // API endpoint URL'i
            string url = $"{baseUrl}api/v1/categories";

            // JSON içeriğimiz

            var requestData = new
            {
                categoryId = request.CategoryId,
                trendyolCategoryId = request.TrendyolCategoryId,
                hepsiburadaCategoryId = 0,
                n11CategoryId = 0,
                amazonCategoryId = 0
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
