using System.Net.Http.Headers;
using System.Text;
using Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace;

namespace Nop.Plugin.Widgets.PharmaBase.Service.Auth
{
    public class BasicAuthentication
    {
        private readonly HttpClient _httpClient;
        public BasicAuthentication(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private static Random random = new Random();
        private static string _randomUser;
        private static string _randomPassword;

        public static string RandomUserName
        {
            get
            {
                if (_randomUser == null)
                {
                    _randomUser = GenerateRandomString(10);
                }
                return _randomUser;
            }
        }
        public static string RandomPassword
        {
            get
            {
                if (_randomPassword == null)
                {
                    _randomPassword = GenerateRandomString(15);
                }
                return _randomPassword;
            }
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        public void Authenticate()
        {
            var username = RandomUserName;
            var password = RandomPassword;
            var authenticationString = $"{username}:{password}";
            var base64EncodeAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodeAuthenticationString);
        }

        public async Task<AddSignUpRequestModel> GetSignUpInfo()
        {
            var info = new AddSignUpRequestModel
            {
                Name = "Name",
                Info = "",
                UserName = RandomUserName,
                Password = RandomPassword
            };

            return info;
        }
    }
}
