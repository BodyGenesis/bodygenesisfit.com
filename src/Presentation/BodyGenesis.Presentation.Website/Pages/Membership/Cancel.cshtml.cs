using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BodyGenesis.Core.Entities;
using BodyGenesis.Core.Services;
using BodyGenesis.Core.UseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Piranha.AspNetCore.Services;

namespace BodyGenesis.Presentation.Website.Pages.Membership
{
    [Authorize("MembershipPolicy")]
    public class CancelModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IEmailSender _emailSender;
        private readonly IOptions<WebsiteOptions> _websiteOptions;

        public CancelModel(IMediator mediator, IEmailSender emailSender, IOptions<WebsiteOptions> websiteOptions)
        {
            _mediator = mediator;
            _emailSender = emailSender;
            _websiteOptions = websiteOptions;
        }

        public Customer Customer { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var auth0UserId = User.FindFirstValue("sub");

            var result = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (result.HasError)
            {
                ViewData["ErrorMessage"] = result.Message;
            }

            Customer = result.Value;

            if (Customer.CurrentMembershipSubscription == null)
            {
                return Redirect("/membership");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var auth0UserId = User.FindFirstValue("sub");

            var result = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (result.HasError)
            {
                ViewData["ErrorMessage"] = result.Message;
            }

            Customer = result.Value;

            Customer.CurrentMembershipSubscription.RequestCancellation();

            await _mediator.Send(new SaveCustomerRequest(Customer));
            await _emailSender.Send(_websiteOptions.Value.EmailRecipients, "Cancellation Request", $"<a href=\"https://{HttpContext.Request.Host}/backoffice/customers/{Customer.Id}\">Click Here to View Customer Record</a>");


            return Redirect("/membership");
        }
    }
}
