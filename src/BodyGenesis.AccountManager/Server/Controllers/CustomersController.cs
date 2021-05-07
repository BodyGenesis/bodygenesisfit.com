using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using BodyGenesis.Core.Entities;
using BodyGenesis.Shared;

namespace BodyGenesis.AccountManager.Server.Controllers
{
    [Route("api/customers")]
    public class CustomersController
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomersController(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Maybe<Customer>> Get(string id, [FromQuery(Name = "$axis")]string axis = null)
        {
            axis = axis ?? string.Empty;

            if (axis.Equals("auth0UserId"))
            {
                var results = await _customerRepository.Query(c => c.Auth0UserId == id);

                if (results.Count == 0)
                {
                    return Maybe<Customer>.None;
                }

                return Maybe<Customer>.From(results.First());
            }

            return await _customerRepository.Get(Guid.Parse(id));
        }

        [HttpPost]
        [Route("")]
        public async Task Save([FromBody]Customer customer)
        {
            customer.MembershipSubscriptions = new List<MembershipSubscription>();
            customer.PaymentMethods = new List<PaymentMethod>();

            var maybeExistingCustomer = await _customerRepository.Get(customer.Id);

            if (maybeExistingCustomer.HasValue)
            {
                var existingCustomer = maybeExistingCustomer.Value;

                customer.MembershipSubscriptions = existingCustomer.MembershipSubscriptions;
                customer.PaymentMethods = existingCustomer.PaymentMethods;
            }

            await _customerRepository.Save(customer);
        }
    }
}
