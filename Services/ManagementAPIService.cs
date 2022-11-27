using metronic_extensions_api.Controllers;
using metronic_extensions_api.Services.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Runtime;
using System.Xml.Linq;

namespace metronic_extensions_api.Services
{

    public class ManagementAPIService
    {



        private readonly HttpClient _client;

        private readonly ILogger<ManagementAPIService> _logger;


        public ManagementAPIService(ILogger<ManagementAPIService> logger, TokenmgmtService service, IHttpClientFactory factory)
        {
            _logger = logger;
            _client = factory.CreateClient("MgmtAPI");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {service.GetToken()}");
        }


        public UserResponse GetUsers(int page, int items_per_page,string? field,string? order)
        {
            try
            {
                string query = "";
                if(field!=null && order != null)
                {
                    int val = order == "asc" ? 1 : -1;
                    query = $"sort={field}:{val}";
                }
                var request = new HttpRequestMessage(HttpMethod.Get, $"users?page={page-1}&per_page={items_per_page}&include_totals=true&{query}");
                HttpResponseMessage response = _client.SendAsync(request).Result;

                var responseData = JsonConvert.DeserializeObject<UserResponse>(response.Content.ReadAsStringAsync().Result);

                var links = new List<Link>();
                var pages = Math.Ceiling(((double)responseData.total / items_per_page));         
                for(int i = 1; i <= pages; i++)
                {
                    links.Add(new Link { label = i.ToString(), page = i });
                }
                responseData.pagination = new Pagination() { items_per_page = items_per_page,page = page,
                    links = links
                };                
                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fetching users failed with the error {ex}");
                throw new Exception($"Fetching users failed with the error {ex}");
            }
        }

    }

}