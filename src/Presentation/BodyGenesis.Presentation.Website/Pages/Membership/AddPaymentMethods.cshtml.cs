using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BodyGenesis.Core.Entities;
using BodyGenesis.Core.UseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace BodyGenesis.Presentation.Website.Pages.Membership
{
    [Authorize("MembershipPolicy")]
    public class AddPaymentMethodsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public AddPaymentMethodsModel(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [BindProperty]
        public PaymentMethod PaymentMethod1 { get; set; }

        [BindProperty]
        public string AccountNumber1 { get; set; }

        [BindProperty]
        public PaymentMethod PaymentMethod2 { get; set; }

        [BindProperty]
        public string AccountNumber2 { get; set; }

        [BindProperty]
        public string SubmitAction { get; set; }

        public IEnumerable<SelectListItem> PaymentMethodTypeOptions => new SelectListItem[]
        {
            new SelectListItem(PaymentMethodType.BankAccount.ToDisplayText(), ((int)PaymentMethodType.BankAccount).ToString()),
            new SelectListItem(PaymentMethodType.CreditCard.ToDisplayText(), ((int)PaymentMethodType.CreditCard).ToString())
        };

        public MembershipSubscription MembershipSubscription { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var auth0UserId = User.FindFirstValue("sub");

            var customerResult = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (customerResult.HasError)
            {
                ViewData["ErrorMessage"] = customerResult.Message;
            }

            var customer = customerResult.Value;

            if (customer.PaymentMethods.Count > 1)
            {
                return Redirect(customer.CurrentMembershipSubscription.AgreementSigned ? "/membership" : "/membership/agreement");
            }

            MembershipSubscription = customer.CurrentMembershipSubscription;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            PaymentMethod1.Type = PaymentMethodType.BankAccount;

            if (PaymentMethod1.Type == PaymentMethodType.GiftCard || PaymentMethod2.Type == PaymentMethodType.GiftCard)
            {
                ViewData["ErrorMessage"] = "Gift cards are not currently supported.";

                return Page();
            }

            var auth0UserId = User.FindFirstValue("sub");

            var customerResult = await _mediator.Send(new GetCustomerForAuth0UserRequest(auth0UserId));

            if (customerResult.HasError)
            {
                ViewData["ErrorMessage"] = customerResult.Message;
            }

            var customer = customerResult.Value;
            var encryptionKey = _configuration["BodyGenesis:Shared:EncryptionKey"];

            if (!SubmitAction.Equals("continue", StringComparison.OrdinalIgnoreCase))
            {
                customer.MembershipSubscriptions.Remove(customer.CurrentMembershipSubscription);

                await _mediator.Send(new SaveCustomerRequest(customer));

                return Redirect("/membership/join");
            }

            PaymentMethod1.SetEncryptedAccountNumber(AccountNumber1, encryptionKey);
            PaymentMethod2.SetEncryptedAccountNumber(AccountNumber2, encryptionKey);

            customer.PaymentMethods.Add(PaymentMethod1);
            customer.PaymentMethods.Add(PaymentMethod2);

            await _mediator.Send(new SaveCustomerRequest(customer));

            return Redirect("/membership/agreement");
        }
    }
}
