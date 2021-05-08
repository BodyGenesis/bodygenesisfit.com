using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using BodyGenesis.AccountManager.Shared.Models;
using BodyGenesis.Core.Entities;
using BodyGenesis.Shared;

namespace BodyGenesis.AccountManager.Server.Controllers
{
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomersController(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Maybe<CustomerDto>> Get(string id)
        {
            Customer customer = null;

            if (id.StartsWith("auth0"))
            {
                var results = await _customerRepository.Query(c => c.Auth0UserId == id);

                if (results.Count > 0)
                {
                    customer = results.First();
                }
            }

            else
            {
                var maybeCustomer = await _customerRepository.Get(Guid.Parse(id));

                if (maybeCustomer.HasValue)
                {
                    customer = maybeCustomer.Value;
                }
            }

            if (customer is null)
            {
                return Maybe<CustomerDto>.None;
            }

            return Maybe<CustomerDto>.From(new CustomerDto
            {
                Address = customer.Address,
                Auth0UserId = customer.Auth0UserId,
                City = customer.City,
                CurrentMembershipSubscription = customer.CurrentMembershipSubscription,
                DateCreated = customer.DateCreated,
                DateDeleted = customer.DateDeleted,
                DateOfBirth = customer.DateOfBirth,
                EmailAddress = customer.EmailAddress,
                Id = customer.Id,
                Name = customer.Name,
                PaymentMethods = customer.PaymentMethods.Select(pm => new PaymentMethodDto
                {
                    AccountNumberHint = pm.AccountNumberHint,
                    ExpirationDate = pm.ExpirationDate,
                    NameOnCard = pm.NameOnCard,
                    Primary = pm.Primary,
                    RoutingNumber = pm.RoutingNumber,
                    Type = pm.Type
                }).ToList(),
                PhoneNumber = customer.PhoneNumber,
                PreferredLocation = customer.PreferredLocation,
                State = customer.State,
                ZipCode = customer.ZipCode
            });
        }

        [HttpPost]
        [Route("")]
        public async Task Save([FromBody]CustomerDto customerDto)
        {
            var maybeExistingCustomer = await _customerRepository.Get(customerDto.Id);

            var customer = maybeExistingCustomer.Value;

            if (customer is null)
            {
                customer = new Customer
                {
                    Auth0UserId = customerDto.Auth0UserId,
                    DateCreated = DateTime.Now,
                    EmailAddress = customerDto.EmailAddress,
                    Id = Guid.NewGuid()
                };
            }

            customer.Address = customerDto.Address;
            customer.City = customerDto.City;
            customer.DateOfBirth = customerDto.DateOfBirth;
            customer.Name = customerDto.Name;
            customer.PhoneNumber = customerDto.PhoneNumber;
            customer.PreferredLocation = customerDto.PreferredLocation;
            customer.State = customerDto.State;
            customer.ZipCode = customerDto.ZipCode;

            await _customerRepository.Save(customer);
        }
    }
}
