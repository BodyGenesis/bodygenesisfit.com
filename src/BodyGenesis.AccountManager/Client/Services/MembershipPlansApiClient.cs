using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using BodyGenesis.Core.Entities;

namespace BodyGenesis.AccountManager.Client.Services
{
    public class MembershipPlansApiClient
    {
        private readonly HttpClient _httpClient;

        public MembershipPlansApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<MembershipPlan>> List()
        {
            return await _httpClient.GetFromJsonAsync<IReadOnlyCollection<MembershipPlan>>("api/membership-plans");
        }
    }
}
