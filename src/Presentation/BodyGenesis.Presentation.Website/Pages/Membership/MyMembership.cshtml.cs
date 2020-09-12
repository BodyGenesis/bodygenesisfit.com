using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using BodyGenesis.Core.Entities;
using BodyGenesis.Core.UseCases;

namespace BodyGenesis.Presentation.Website.Pages.Membership
{
    public class MyMembershipModel : PageModel
    {
        private readonly IMediator _mediator;

        public MyMembershipModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Customer Customer { get; private set; }

        [BindProperty]
        public string CustomerName { get; set; }

        [BindProperty]
        public string CustomerEmail { get; set; }

        [BindProperty]
        public DateTime? CustomerDateOfBirth { get; set; }

        [BindProperty]
        public string CustomerPhoneNumber { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var auth0UserId = User.FindFirstValue("sub");

            var result = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (result.HasError)
            {
                ViewData["ErrorMessage"] = result.Message;
            }

            Customer = result.Value;
            CustomerDateOfBirth = Customer.DateOfBirth;
            CustomerEmail = Customer.EmailAddress;
            CustomerName = Customer.Name;
            CustomerPhoneNumber = Customer.PhoneNumber;

            if (Customer.CurrentMembershipSubscription != null && !Customer.CurrentMembershipSubscription.AgreementSigned)
            {
                return Redirect("/membership/payment-methods/add-initial");
            }

            return Page();
        }

        public async Task OnPostAsync()
        {
            var auth0UserId = User.FindFirstValue("sub");

            var result = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (result.HasError)
            {
                ViewData["ErrorMessage"] = result.Message;
            }

            Customer = result.Value;

            Customer.Name = CustomerName;
            Customer.EmailAddress = CustomerEmail;
            Customer.DateOfBirth = CustomerDateOfBirth;
            Customer.PhoneNumber = CustomerPhoneNumber;

            await _mediator.Send(new SaveCustomerRequest(Customer));
        }
    }
}
