using System;

using MediatR;

using BodyGenesis.Core.Entities;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class GetCustomerForAuth0UserRequest : IRequest<Result<Customer>>
    {
        public GetCustomerForAuth0UserRequest(string auth0UserId)
        {
            Auth0UserId = auth0UserId ?? throw new ArgumentNullException(nameof(auth0UserId));
        }

        public string Auth0UserId { get; }
    }
}
