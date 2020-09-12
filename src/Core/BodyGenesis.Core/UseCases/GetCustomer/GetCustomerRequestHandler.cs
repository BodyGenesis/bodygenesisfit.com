using System.Threading;
using System.Threading.Tasks;

using MediatR;

using BodyGenesis.Core.Entities;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class GetCustomerRequestHandler : IRequestHandler<GetCustomerRequest, Result<Customer>>
    {
        private readonly IRepository<Customer> _customerRepository;

        public GetCustomerRequestHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Customer>> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
        {
            var maybeCustomer = await _customerRepository.Get(request.CustomerId);

            if (!maybeCustomer.HasValue)
            {
                return Result<Customer>.Error($"Unable to find a customer with the ID '{request.CustomerId}'.");
            }

            return Result<Customer>.Success(maybeCustomer.Value);
        }
    }
}
