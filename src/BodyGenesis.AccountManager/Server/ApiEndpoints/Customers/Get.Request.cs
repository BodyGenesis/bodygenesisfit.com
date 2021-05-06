using Microsoft.AspNetCore.Mvc;

namespace BodyGenesis.AccountManager.Server.ApiEndpoints.Customers
{
    public class GetCustomerRequest
    {
        [FromRoute(Name = "id")]
        public string Id { get; } = string.Empty;

        [FromQuery(Name = "auth0UserId")]
        public string Auth0UserId { get; } = string.Empty;
    }
}
