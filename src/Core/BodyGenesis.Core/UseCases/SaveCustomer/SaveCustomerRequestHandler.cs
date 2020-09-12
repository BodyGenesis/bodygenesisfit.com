using System.Threading;
using System.Threading.Tasks;

using MediatR;

using BodyGenesis.Core.Entities;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.UseCases
{
    public class SaveCustomerRequestHandler : IRequestHandler<SaveCustomerRequest, Result>
    {
        private readonly IRepository<Customer> _customerRepository;

        public SaveCustomerRequestHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result> Handle(SaveCustomerRequest request, CancellationToken cancellationToken)
        {
            await _customerRepository.Save(request.Customer);

            return Result.Success();
        }
    }
}
