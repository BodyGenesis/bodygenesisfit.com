using MediatR;

using BodyGenesis.Core.Entities;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class SaveCustomerRequest : IRequest<Result>
    {
        public SaveCustomerRequest(Customer customer)
        {
            Customer = customer;
        }

        public Customer Customer { get; }
    }
}
