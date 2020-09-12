using System;

using MediatR;

using BodyGenesis.Core.Entities;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class GetCustomerRequest : IRequest<Result<Customer>>
    {
        public GetCustomerRequest(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
    }
}
