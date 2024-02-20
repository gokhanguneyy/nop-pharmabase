using System.Net.Http.Headers;
using System.Text;
using Nop.Plugin.Widgets.PharmaBase.Helper;
using Nop.Plugin.Widgets.PharmaBase.Models;
using Nop.Plugin.Widgets.PharmaBase.Models.Attributes;
using Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace;
using Nop.Plugin.Widgets.PharmaBase.Models.Order;

namespace Nop.Plugin.Widgets.PharmaBase.Service.MarketPlace
{
    public class MarketPlaceCategoryService : IMarketPlaceCategoryServiceInterface
    {
        private readonly ISerializerHelper _serializerHelper;

        public MarketPlaceCategoryService(ISerializerHelper serializerHelper)
        {
            _serializerHelper = serializerHelper;
        }

     

        public async Task<PagingResponseModel<List<BrandResponse>>> GetBrandsAsync()
        {
            using (var client = new HttpClient())
            {
                //TODO start page & marketplacetype & limit alınacak
                var url = "https://localhost:44309/api/v1/brands?startPage=0&limit=10&marketPlaceType=Trendyol";
                var username = "demo_admin";
                var password = "demo_password";

                var authenticationString = $"{username}:{password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);


                var response = await client.GetAsync(url);
                var jsonResult = await response.Content.ReadAsStringAsync();
                //List<CategoriesResponse>
                //PagingResponseModel<List<CategoriesResponse>>
                var result = _serializerHelper.DeserializeObject<PagingResponseModel<List<BrandResponse>>>(jsonResult);

                return result;

            }
        }

        public async Task<PagingResponseModel<List<MarketPlaceCategoriesResponse>>> GetCategoriesAsync()
        {
            using (var client = new HttpClient())
            {
                //TODO start page & marketplacetype & limit alınacak
                var url = "https://localhost:44309/api/v1/categories?startPage=0&limit=10&marketPlaceType=Trendyol";
                var username = "demo_admin";
                var password = "demo_password";

                var authenticationString = $"{username}:{password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);


                var response = await client.GetAsync(url);
                var jsonResult = await response.Content.ReadAsStringAsync();
                //List<CategoriesResponse>
                //PagingResponseModel<List<CategoriesResponse>>
                var result = _serializerHelper.DeserializeObject<PagingResponseModel<List<MarketPlaceCategoriesResponse>>>(jsonResult);

                return result;

            }

        }

        public async Task<AttributesResponse> GetAttributesByCategoryId(int categoryId)
        {
            using (var client = new HttpClient())
            {
                //TODO start page & marketplacetype & limit alınacak
                var url = $"https://localhost:44309/api/v1/attributes?categoryId={categoryId}&marketPlaceType=Trendyol";
                var username = "demo_admin";
                var password = "demo_password";

                var authenticationString = $"{username}:{password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);


                var response = await client.GetAsync(url);
                var jsonResult = await response.Content.ReadAsStringAsync();
                //List<CategoriesResponse>
                //PagingResponseModel<List<CategoriesResponse>>
                var result = _serializerHelper.DeserializeObject<AttributesResponse>(jsonResult);

                return result;

            }
        }


    }
}
