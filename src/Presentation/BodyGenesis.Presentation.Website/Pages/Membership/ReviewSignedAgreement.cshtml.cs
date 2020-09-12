using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BodyGenesis.Core.UseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BodyGenesis.Presentation.Website.Pages.Membership
{
    [Authorize("MembershipPolicy")]
    public class ReviewSignedAgreementModel : PageModel
    {
        private readonly IMediator _mediator;

        public ReviewSignedAgreementModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string AgreementHtml { get; set; } = string.Empty;
        public DateTime AgreementDate { get; set; }

        public async Task OnGetAsync()
        {
            var auth0UserId = User.FindFirstValue("sub");

            var result = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (result.HasError)
            {
                ViewData["ErrorMessage"] = result.Message;
            }

            var customer = result.Value;

            AgreementHtml = customer.CurrentMembershipSubscription.AgreementData;
            AgreementDate = customer.CurrentMembershipSubscription.DateAgreementSigned.Value;
        }
    }
}
