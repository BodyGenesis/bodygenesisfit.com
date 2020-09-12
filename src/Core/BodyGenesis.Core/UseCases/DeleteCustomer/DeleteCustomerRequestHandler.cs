using System.Threading;
using System.Threading.Tasks;

using MediatR;

using BodyGenesis.Shared;
using BodyGenesis.Core.Entities;

namespace BodyGenesis.Core.UseCases
{
    public class DeleteCustomerRequestHandler : IRequestHandler<DeleteCustomerRequest, Result>
    {
        private readonly IRepository<Customer> _customerRepository;

        public DeleteCustomerRequestHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
        {
            await _customerRepository.Delete(request.CustomerId);

            return Result.Success();
        }
    }
}
