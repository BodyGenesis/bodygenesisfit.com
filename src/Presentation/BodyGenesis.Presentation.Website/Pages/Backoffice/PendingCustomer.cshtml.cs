using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BodyGenesis.Core.Entities;
using BodyGenesis.Core.UseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BodyGenesis.Presentation.Website.Pages.Backoffice
{
    [Authorize("BackofficePolicy")]
    public class PendingCustomerModel : PageModel
    {
        private readonly IMediator _mediator;

        public PendingCustomerModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Customer Customer { get; private set; }

        [BindProperty]
        public Guid CustomerId { get; set; }

        [BindProperty]
        public string SubmitAction { get; set; }

        public async Task OnGetAsync(Guid customerId)
        {
            CustomerId = customerId;

            var result = await _mediator.Send(new GetCustomerRequest(CustomerId));

            if (result.HasError)
            {
                ViewData["ErrorMessage"] = result.Message;
            }

            Customer = result.Value;
        }

        public async Task OnPostAsync()
        {
            var result = await _mediator.Send(new GetCustomerRequest(CustomerId));

            if (result.HasError)
            {
                ViewData["ErrorMessage"] = result.Message;
            }

            Customer = result.Value;

            if (SubmitAction.Equals("confirm keyfob pick-up", StringComparison.OrdinalIgnoreCase))
            {
                Customer.CurrentMembershipSubscription.Active = true;
            }

            else if (SubmitAction.Equals("confirm cancellation", StringComparison.OrdinalIgnoreCase))
            {
                Customer.CurrentMembershipSubscription.Cancel();
                Customer.PaymentMethods.Clear();
            }

            await _mediator.Send(new SaveCustomerRequest(Customer));
        }
    }
}
