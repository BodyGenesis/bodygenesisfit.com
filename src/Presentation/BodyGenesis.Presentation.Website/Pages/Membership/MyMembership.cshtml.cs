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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BodyGenesis.Presentation.Website.Pages.Membership
{
    [Authorize("MembershipPolicy")]
    public class MyMembershipModel : PageModel
    {
        private readonly IMediator _mediator;

        public MyMembershipModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public IEnumerable<SelectListItem> StateOptions => new string[]
        {
            "Alabama",
            "Alaska",
            "Arizona",
            "Arkansas",
            "California",
            "Colorado",
            "Connecticut",
            "Delaware",
            "Florida",
            "Georgia",
            "Hawaii",
            "Idaho",
            "Illinois",
            "Indiana",
            "Iowa",
            "Kansas",
            "Kentucky",
            "Louisiana",
            "Maine",
            "Maryland",
            "Massachusetts",
            "Michigan",
            "Minnesota",
            "Mississippi",
            "Missouri",
            "Montana",
            "Nebraska",
            "Nevada",
            "New Hampshire",
            "New Jersey",
            "New Mexico",
            "New York",
            "North Carolina",
            "North Dakota",
            "Ohio",
            "Oklahoma",
            "Oregon",
            "Pennsylvania",
            "Rhode Island",
            "South Carolina",
            "South Dakota",
            "Tennessee",
            "Texas",
            "Utah",
            "Vermont",
            "Virginia",
            "Washington",
            "West Virginia",
            "Wisconsin",
            "Wyoming"
        }.Select(s => new SelectListItem(s, s));

        public IEnumerable<SelectListItem> PreferredLocationOptions => new string[]
        {
            "Saxonburg",
            "New Castle/Shenango"
        }.Select(s => new SelectListItem(s, s));

        public async Task<IActionResult> OnGetAsync()
        {
            var auth0UserId = User.FindFirstValue("sub");

            var result = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (result.HasError)
            {
                ViewData["ErrorMessage"] = result.Message;
            }

            Customer = result.Value;

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

            var customer = result.Value;

            customer.Name = Customer.Name;
            customer.EmailAddress = Customer.EmailAddress;
            customer.DateOfBirth = Customer.DateOfBirth;
            customer.PhoneNumber = Customer.PhoneNumber;
            customer.Address = Customer.Address;
            customer.City = Customer.City;
            customer.State = Customer.State;
            customer.ZipCode = Customer.ZipCode;
            customer.PreferredLocation = Customer.PreferredLocation;

            Customer = customer;

            await _mediator.Send(new SaveCustomerRequest(customer));
        }
    }
}
