﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BodyGenesis.Core.Services;
using BodyGenesis.Core.UseCases;
using BodyGenesis.Presentation.Website.Cms.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Piranha;
using Piranha.AspNetCore.Models;
using Piranha.AspNetCore.Services;
using Piranha.Extend;
using Piranha.Extend.Blocks;

namespace BodyGenesis.Presentation.Website.Pages
{
    public class AgreementPageModel : SinglePage<AgreementPage>
    {
        private readonly IMediator _mediator;
        private readonly IEmailSender _emailSender;

        public AgreementPageModel(IApi api, IModelLoader modelLoader, IMediator mediator, IEmailSender emailSender)
            : base(api, modelLoader)
        {
            _mediator = mediator;
            _emailSender = emailSender;
        }

        [BindProperty]
        public Guid PageId { get; set; }

        [BindProperty]
        public string SubmitAction { get; set; }

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
                var page = await _api.Pages.GetByIdAsync<AgreementPage>(PageId);
                var agreementData = page.Blocks.Aggregate(string.Empty, (previous, block) => $"{previous}<br />{_ExtractBlockText(block)}");

                customer.CurrentMembershipSubscription.SignAgreement(agreementData);

                await _emailSender.Send(new string[] { "josh.johnson@leafyacre.com" }, "New BodyGenesis Membership", $"<a href=\"https://{HttpContext.Request.Host}/backoffice/customers/{customer.Id}\">Click Here to View Customer Record</a>");
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