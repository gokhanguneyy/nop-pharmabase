using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Nop.Plugin.Widgets.PharmaBase.Models.Order;

namespace Nop.Plugin.Widgets.PharmaBase.Services
{
    public class MarketPlaceOrderService
    {

        public async Task<MarketPlaceOrderData> GetOrders()
        {
            using (var client = new HttpClient())
            {
                var url = "https://localhost:44309/api/v1/orders";
                var username = "demo_admin";
                var password = "demo_password";

                var authenticationString = $"{username}:{password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);


                var response = await client.GetAsync(url);
                var responseData = await response.Content.ReadAsStringAsync();
                var jsonData = JObject.Parse(responseData);
                var orders = new MarketPlaceOrderData();
                if (jsonData["data"] is JArray dataArray)
                {
                    var sampleOrder = new MarketPlaceOrderDetail();
                    foreach (var item in dataArray)
                    {
                        sampleOrder.Id = (string?)item["id"];
                        sampleOrder.OrderNumber = (string?)item["orderNumber"];
                        sampleOrder.TotalPrice = (int)item["totalPrice"];
                        sampleOrder.CustomerFirstName = (string?)item["customerFirstName"];
                        sampleOrder.CustomerLastName = (string?)item["customerLastName"];
                        sampleOrder.CustomerEmail = (string?)item["customerEmail"];
                        sampleOrder.CargoTrackingNumber = (int)item["cargoTrackingNumber"];
                        sampleOrder.CargoTrackingLink = (string?)item["cargoTrackingLink"];
                        sampleOrder.CargoProviderName = (string?)item["cargoProviderName"];
                        sampleOrder.OrderDate = (string?)item["orderDate"];
                        sampleOrder.CurrencyCode = (string?)item["currencyCode"];
                        sampleOrder.Status = (string?)item["status"];
                        sampleOrder.MarketPlaceType = (string?)item["marketPlaceType"];

                        if (string.IsNullOrEmpty(sampleOrder.Status))
                        {
                            sampleOrder.Status = "İşleniyor";

                        }
                        orders.Data.Add(sampleOrder);
                    }
                }
                orders.totalRecords = orders.Data.Count();
                //return _serializerHelper.DeserializeObject<HepsiburadaOrderResponse>(jsonResult);


                return orders;
            }
        }
    }
}
