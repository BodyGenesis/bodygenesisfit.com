using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Ardalis.ApiEndpoints;

using Microsoft.AspNetCore.Mvc;

using BodyGenesis.Core.Entities;
using BodyGenesis.Shared;

namespace BodyGenesis.AccountManager.Server.ApiEndpoints.Customers
{
    public class Get : BaseAsyncEndpoint
        .WithRequest<GetCustomerRequest>
        .WithResponse<Maybe<Customer>>
    {
        private readonly IRepository<Customer> _customerRepository;

        [HttpGet]
        [Route("api/customers/{id}")]
        public override async Task<ActionResult<Maybe<Customer>>> HandleAsync(GetCustomerRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                var results = await _customerRepository.Query(c => c.Auth0UserId == request.Auth0UserId);

                if (results.Count == 0)
                {
                    return Maybe<Customer>.None;
                }

                return Maybe<Customer>.From(results.First());
            }

            return await _customerRepository.Get(Guid.Parse(request.Id));
        }
    }
}
