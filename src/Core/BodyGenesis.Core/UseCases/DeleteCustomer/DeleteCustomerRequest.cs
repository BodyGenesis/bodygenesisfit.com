using System;

using MediatR;

using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class DeleteCustomerRequest : IRequest<Result>
    {
        public DeleteCustomerRequest(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
    }
}
