using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BodyGenesis.Core.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BodyGenesis.Infrastructure.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly ISendGridClient _sendGridClient;

        public SendGridEmailSender(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task Send(IEnumerable<string> toAddresses, string subject, string body)
        {
            var message = MailHelper.CreateSingleEmailToMultipleRecipients(new EmailAddress("noreply@bodygenesisfit.com", "BodyGenesis"), toAddresses.Select(a => new EmailAddress(a)).ToList(), subject, string.Empty, body);

            await _sendGridClient.SendEmailAsync(message);
        }
    }
}
