using System;

using MediatR;

using BodyGenesis.Core.Entities;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class EnsureCustomerForAuth0UserRequest : IRequest<Result<Customer>>
    {
        public EnsureCustomerForAuth0UserRequest(string auth0UserId, string name, string emailAddress)
        {
            Auth0UserId = auth0UserId ?? throw new ArgumentNullException(nameof(auth0UserId));
            Name = name ?? string.Empty;
            EmailAddress = emailAddress ?? string.Empty;
        }

        public string Auth0UserId { get; }
        public string Name { get; }
        public string EmailAddress { get; }
    }
}
