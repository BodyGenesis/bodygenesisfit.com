using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using BodyGenesis.Core.Entities;
using BodyGenesis.Core.Entities.Queries;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class GetCustomerForAuth0UserRequestHandler : IRequestHandler<GetCustomerForAuth0UserRequest, Result<Customer>>
    {
        private readonly IRepository<Customer> _customerRepository;

        public GetCustomerForAuth0UserRequestHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Customer>> Handle(GetCustomerForAuth0UserRequest request, CancellationToken cancellationToken)
        {
            var results = await _customerRepository.Query(new CustomerByAuth0UserIdQuery(request.Auth0UserId));

            if (results.Count > 1 || results.Count == 0)
            {
                return Result<Customer>.Error($"A customer could not be found for Auth0 user '{request.Auth0UserId}' or more than one match was found.");
            }

            return Result<Customer>.Success(results.First());
        }
    }
}
