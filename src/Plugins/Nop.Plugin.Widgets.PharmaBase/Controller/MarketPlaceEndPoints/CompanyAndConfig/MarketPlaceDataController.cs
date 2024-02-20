using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace;
using Nop.Plugin.Widgets.PharmaBase.Service.Auth;
using Nop.Plugin.Widgets.PharmaBase.Services;
using Nop.Services.Configuration;
using Nop.Web.Framework.Controllers;


namespace Nop.Plugin.Widgets.PharmaBase.Controller.MarketPlaceEndPoints.CompanyAndConfig
{
    public class MarketPlaceDataController : BasePluginController
    {
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly HttpClient _httpClient;

        public MarketPlaceDataController(IStoreContext storeContext, ISettingService settingService, HttpClient httpClient)
        {
            _settingService = settingService;
            _storeContext = storeContext;
            _httpClient = httpClient;
        }

        string _result;
        [HttpPost]
        public async Task<IActionResult> SendAuthInfo()
        {
            
            try
            {
                var store = await _storeContext.GetCurrentStoreAsync();
                var pharmaBaseSettings = await _settingService.LoadSettingAsync<PharmaBaseSettings>(store.Id);
                var baseUrl = pharmaBaseSettings.Url;

                var sign = new BasicAuthentication(_httpClient);
                var signUpModel = sign.GetSignUpInfo();
                var item = new AddSignUpRequestModel
                {
                    Name = signUpModel.Result.Name,
                    Info = signUpModel.Result.Info,
                    UserName = signUpModel.Result.UserName,
                    Password = signUpModel.Result.Password
                };

                var url = $"{baseUrl}api/v1/company/sign-up";
                //var url = "https://localhost:44309/api/v1/company/sign-up";
                var json = JsonConvert.SerializeObject(item);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, data);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    sign.Authenticate();

                    var pharmaBaseSettingss = await _settingService.LoadSettingAsync<PharmaBaseSettings>(store.Id);
                    var baseUrl2 = pharmaBaseSettingss.Url;
                    var configurePageData = await SendConfigData();
                    var urlForConfig = $"{baseUrl2}api/v1/company/config";
                    //var urlForConfig = "https://localhost:44309/api/v1/company/config";
                    foreach (var jsonData in configurePageData)
                    {
                        var jsonOfConfig = JsonConvert.SerializeObject(jsonData);
                        var dataOfConfig = new StringContent(jsonOfConfig, Encoding.UTF8, "application/json");
                        try
                        {
                            var responseOfConfig = await _httpClient.PostAsync(urlForConfig, dataOfConfig);
                            var contentOfConfig = await responseOfConfig.Content.ReadAsStringAsync();
                            if (responseOfConfig.IsSuccessStatusCode)
                            {
                                _result = contentOfConfig;
                            }
                            else
                            {
                                return StatusCode((int)response.StatusCode, content);
                            }
                        }
                        catch (Exception)
                        {

                            return StatusCode(500, "InternalServerError for Config");
                        }
                    }
                }
                else
                {
                    return StatusCode((int)response.StatusCode, content);
                }

                return Ok(_result);
            }
            catch (Exception)
            {
                return StatusCode(500, "InternalServerError for Company");
            }
        }

        public async Task<MarketPlaceModel[]> SendConfigData()
        {

            var store = await _storeContext.GetCurrentStoreAsync();
            var pharmaBaseSettings = await _settingService.LoadSettingAsync<PharmaBaseSettings>(store.Id);

            var data = new MarketPlaceModel[]
            {
                new MarketPlaceModel
                {
                    UserName = pharmaBaseSettings.trendyolDeveloperUserName,
                    Password = pharmaBaseSettings.trendyolPassword,
                    MarketPlaceSupplierId = pharmaBaseSettings.trendyolSupplierId,
                    IsActive = true,
                    MarketPlaceId = 1,
                },
                new MarketPlaceModel
                {
                    UserName = pharmaBaseSettings.hepsiburadaDeveloperUserName,
                    Password = pharmaBaseSettings.hepsiburadaPassword,
                    MarketPlaceSupplierId = pharmaBaseSettings.hepsiburadaSupplierId,
                    IsActive = true,
                    MarketPlaceId = 2,
                },
                new MarketPlaceModel
                {
                    UserName = pharmaBaseSettings.amazonDeveloperUserName,
                    Password = pharmaBaseSettings.amazonPassword,
                    MarketPlaceSupplierId = pharmaBaseSettings.amazonSupplierId,
                    IsActive = true,
                    MarketPlaceId = 3,
                },
            };

            return data;
        }
        [HttpPost]
        public virtual async Task<IActionResult> OrderList()
        {
            MarketPlaceOrderService service = new MarketPlaceOrderService();
            var model = await service.GetOrders();

            return Json(model);
        }
    }
}
