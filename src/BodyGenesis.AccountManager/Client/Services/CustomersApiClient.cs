using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using BodyGenesis.Core.Entities;
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

        public async Task<Maybe<Customer>> Get(string id, string axis = "")
        {
            return await _httpClient.GetFromJsonAsync<Maybe<Customer>>($"api/customers/{id}{(string.IsNullOrWhiteSpace(axis) ? "" : $"?$axis={axis}")}");
        }

        public async Task<Maybe<Customer>> GetByAuth0UserId(string auth0UserId)
        {
            return await _httpClient.GetFromJsonAsync<Maybe<Customer>>($"api/customers/{auth0UserId}?$axis=auth0UserId");
        }
    }
}
