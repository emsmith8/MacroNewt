using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MacroNewt.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();

            List<string> subjectList = new List<string> { "User Question", "User Feedback", "User Other" };

            if (subjectList.Contains(subject))
            {
                msg.From = new EmailAddress("Accounts@MacroNewt.com", "MacroNewt User");
                msg.Subject = subject;
                msg.PlainTextContent = message;
                msg.HtmlContent = message;

                msg.AddTo(new EmailAddress("evan.matthew.smith@gmail.com"));
            }
            else
            {
                msg.From = new EmailAddress("Accounts@MacroNewt.com", "MacroNewt Accounts");
                msg.Subject = subject;
                msg.PlainTextContent = message;
                msg.HtmlContent = message;

                msg.AddTo(new EmailAddress(email));
            }

            string _b64 = Convert.ToBase64String(File.ReadAllBytes("wwwroot/images/fullLogoCenteringTest.png"));

            Attachment inlineLogo = new Attachment()
            {
                Content = _b64,
                Type = "image/png",
                Filename = "~/images/fullLogoCenteringTest.png",
                Disposition = "inline",
                ContentId = "LogoImage"
            };

            msg.AddAttachment(inlineLogo);


            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }


    }
}
