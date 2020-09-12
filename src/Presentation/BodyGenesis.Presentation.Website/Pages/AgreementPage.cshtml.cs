using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BodyGenesis.Core.Entities;
using BodyGenesis.Core.Services;
using BodyGenesis.Core.UseCases;
using BodyGenesis.Presentation.Website.Cms.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Piranha;
using Piranha.AspNetCore.Models;
using Piranha.AspNetCore.Services;
using Piranha.Extend;
using Piranha.Extend.Blocks;

namespace BodyGenesis.Presentation.Website.Pages
{
    [Authorize("MembershipPolicy")]
    public class AgreementPageModel : SinglePage<AgreementPage>
    {
        private readonly IMediator _mediator;
        private readonly IEmailSender _emailSender;
        private readonly IOptions<WebsiteOptions> _websiteOptions;

        public AgreementPageModel(IApi api, IModelLoader modelLoader, IMediator mediator, IEmailSender emailSender, IOptions<WebsiteOptions> websiteOptions)
            : base(api, modelLoader)
        {
            _mediator = mediator;
            _emailSender = emailSender;
            _websiteOptions = websiteOptions;
        }

        [BindProperty]
        public Guid PageId { get; set; }

        [BindProperty]
        public string SubmitAction { get; set; }

        [BindProperty]
        public string Signer { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var auth0UserId = User.FindFirstValue("sub");

            var customerResult = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (customerResult.HasError)
            {
                ViewData["ErrorMessage"] = customerResult.Message;
            }

            var customer = customerResult.Value;

            if (customer.CurrentMembershipSubscription == null)
            {
                return Redirect("/membership");
            }

            if (SubmitAction.Equals("agree", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(Signer))
                {
                    ViewData["ErrorMessage"] = "Please sign by typing your full name at the bottom of the agreement.";

                    return await base.OnGet(PageId);
                }

                var membership = customer.CurrentMembershipSubscription;
                var page = await _api.Pages.GetByIdAsync<AgreementPage>(PageId);
                var agreementData = page.Blocks.Aggregate(string.Empty, (previous, block) => $"{previous}<br />{_ExtractBlockText(block)}");

                agreementData = string.Concat($"<strong>Membership Plan:</strong>&nbsp;{membership.Plan.Name}<br /><strong>Rate:</strong>&nbsp;${membership.Plan.GetRateForQuantity(membership.Quantity)} {membership.Plan.BillingPeriod.ToDisplayText()}<br />", agreementData);

                customer.CurrentMembershipSubscription.SignAgreement(agreementData, Signer);

                await _emailSender.Send(_websiteOptions.Value.EmailRecipients, $"New BodyGenesis Membership: {customer.Name}", $"<a href=\"https://{HttpContext.Request.Host}/backoffice/customers/{customer.Id}\">Click Here to View Customer Record</a>");
            }

            else
            {
                customer.MembershipSubscriptions.Remove(customer.CurrentMembershipSubscription);
            }

            await _mediator.Send(new SaveCustomerRequest(customer));

            return Redirect("/membership");
        }

        private string _ExtractBlockText(Block block)
        {
            if (block is HtmlBlock htmlBlock)
            {
                return htmlBlock.Body.Value;
            }

            else if (block is QuoteBlock quoteBlock)
            {
                return quoteBlock.Body.Value;
            }

            else if (block is TextBlock textBlock)
            {
                return textBlock.Body.Value;
            }

            return $"[CMS_Block:{block.Id}]";
        }
    }
}
