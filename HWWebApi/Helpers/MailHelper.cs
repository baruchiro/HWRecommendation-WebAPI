using System;
using System.Collections.Generic;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HWWebApi.Helpers
{
    internal class MailHelper
    {
        private static readonly string ApiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
        public static async void Send(string subject, string content)
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

            var client = new SendGridClient(ApiKey);
            await client.SendEmailAsync(message);
        }
    }
}