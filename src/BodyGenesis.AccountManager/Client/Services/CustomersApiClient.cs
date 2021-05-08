using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using BodyGenesis.AccountManager.Shared.Models;
using BodyGenesis.Shared;

namespace BodyGenesis.AccountManager.Client.Services
{
    public class CustomersApiClient
    {
        private readonly HttpClient _httpClient;

        public CustomersApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Maybe<CustomerDto>> Get(string id)
        {
            return await _httpClient.GetFromJsonAsync<Maybe<CustomerDto>>($"api/customers/{id}");
        }

        public async Task Save(CustomerDto customer)
        {
            await _httpClient.PostAsJsonAsync("api/customers", customer);
        }
    }
}
