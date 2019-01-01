using System;
using System.Collections.Generic;
using System.Linq;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace HWWebApi.Helpers
{
    class MailHelper
    {
        private static readonly string apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
        public async static void Send(string subject, string content)
        {
            var message = new SendGridMessage();
            message.SetFrom("do-not-reply@HWRecommendation.com", "Mail from HWRecommendation");

            var recipients = new List<EmailAddress>
            {
                new EmailAddress("baruchiro@gmail.com", "Baruch Rothkoff")
            };

            message.AddTos(recipients);

            message.SetSubject(subject);
            message.AddContent(MimeType.Text, content);

            var client = new SendGridClient(apiKey);
            await client.SendEmailAsync(message);
        }
    }
}