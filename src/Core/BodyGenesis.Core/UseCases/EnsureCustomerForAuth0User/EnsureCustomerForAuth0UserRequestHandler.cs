using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using BodyGenesis.Core.Entities;
using BodyGenesis.Core.Entities.Queries;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class EnsureCustomerForAuth0UserRequestHandler : IRequestHandler<EnsureCustomerForAuth0UserRequest, Result<Customer>>
    {
        private readonly IRepository<Customer> _customerRepository;

        public EnsureCustomerForAuth0UserRequestHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Customer>> Handle(EnsureCustomerForAuth0UserRequest request, CancellationToken cancellationToken)
        {
            var results = await _customerRepository.Query(new CustomerByAuth0UserIdQuery(request.Auth0UserId));

            Customer customer;

            if (results.Count > 1)
            {
                return Result<Customer>.Error($"A customer could not be found for Auth0 user '{request.Auth0UserId}' or more than one match was found.");
            }

            else if (results.Count == 1)
            {
                customer = results.First();

                customer.Name = request.Name;
                customer.EmailAddress = request.EmailAddress;
            }

            else
            {
                customer = new Customer
                {
                    Auth0UserId = request.Auth0UserId,
                    Name = request.Name,
                    EmailAddress = request.EmailAddress
                };
            }

            await _customerRepository.Save(customer);

            return Result<Customer>.Success(customer);
        }
    }
}
