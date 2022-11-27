using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace metronic_extensions_api.Services
{

    public class TokenInformation
    {
        public string? access_token { get; set; }

        public int expiryTime { get; set; }

        public string? scope { get; set; }

        public string? token_type { get; set; }

        public DateTime issuedTime { get; set; }
    }
    public class TokenmgmtService
    {



        private AppSettings _settings;

        private TokenInformation? tokenData;

        private readonly HttpClient _client;
        public TokenmgmtService(IOptions<AppSettings> settings, IHttpClientFactory factory)
        {
            _settings = settings.Value;
            tokenData = null;
            _client = factory.CreateClient();
        }

        public string GetToken()
        {
            if (RefreshToken())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", _settings.ClientId },
                    { "client_secret", _settings.ClientSecret },
                    { "audience", _settings.Audience }
                };
                var request = new HttpRequestMessage(HttpMethod.Post, _settings.AuthUrl);
                request.Content = new FormUrlEncodedContent(parameters);
                HttpResponseMessage response = _client.SendAsync(request).Result;

                tokenData = JsonConvert.DeserializeObject<TokenInformation>(response.Content.ReadAsStringAsync().Result);
                if (tokenData != null)
                {
                    tokenData.issuedTime = DateTime.UtcNow;
                }
            }
            return tokenData.access_token;
        }

        public bool RefreshToken()
        {
            if (tokenData == null || tokenData.access_token == null)
            {
                return true;
            }
            else if ((DateTime.UtcNow - tokenData.issuedTime).TotalSeconds >= tokenData.expiryTime)
            {
                return true;
            }
            return false;
        }
    }


}
