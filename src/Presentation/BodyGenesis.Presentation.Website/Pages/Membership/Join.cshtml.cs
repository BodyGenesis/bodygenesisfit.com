using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BodyGenesis.Core.Entities;
using BodyGenesis.Core.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.AspNetCore.Services;

namespace BodyGenesis.Presentation.Website.Pages.Membership
{
    public class JoinModel : PageModel
    {
        private readonly IMediator _mediator;

        public JoinModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Customer Customer { get; private set; }
        public IReadOnlyCollection<MembershipPlan> MembershipPlans { get; private set; }

        [BindProperty]
        public Guid MembershipPlanId { get; set; }

        [BindProperty]
        public int Quantity { get; set; } = 2;

        public async Task<IActionResult> OnGetAsync()
        {
            var auth0UserId = User.FindFirstValue("sub");

            var customerResult = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (customerResult.HasError)
            {
                ViewData["ErrorMessage"] = customerResult.Message;
            }

            Customer = customerResult.Value;

            if (Customer.CurrentMembershipSubscription != null)
            {
                return Redirect("/membership");
            }

            var plansResult = await _mediator.Send(new ListMembershipPlansRequest());

            MembershipPlans = plansResult.Value;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var auth0UserId = User.FindFirstValue("sub");

            var customerResult = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (customerResult.HasError)
            {
                ViewData["ErrorMessage"] = customerResult.Message;
            }

            Customer = customerResult.Value;

            if (Customer.CurrentMembershipSubscription != null)
            {
                return Redirect("/membership");
            }

            var plansResult = await _mediator.Send(new ListMembershipPlansRequest());

            MembershipPlans = plansResult.Value;

            var selectedMembership = MembershipPlans.First(mp => mp.Id == MembershipPlanId);

            Customer.MembershipSubscriptions.Add(new MembershipSubscription
            {
                Plan = selectedMembership,
                Quantity = selectedMembership.HasQuantityBasedRates ? Quantity : 1,
                StartDate = DateTime.Now
            });

            await _mediator.Send(new SaveCustomerRequest(Customer));

            return Redirect("/membership/payment-methods/add-initial");
        }
    }
}
