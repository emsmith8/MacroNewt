using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            if (subject.Equals("User message"))
            {
                msg.From = new EmailAddress(email, "MacroNewt User");
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

//var contentID = "LogoImage";
//var inlineLogo = new Attachment("~/images/fullLogoCenteringTest.svg");
//inlineLogo.ContentId = contentID;
//            inlineLogo.ContentDisposition.Inline = true;
//            inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

//            MailMessage msg = new MailMessage();

//msg.IsBodyHtml = true;
//            msg.Attachments.Add(inlineLogo);
//            msg.Body = "<div><img src=\"cid:" + contentID + "\"> alt='siteLogo' title='Logo' style='display:block' height='300' width='600' /></div>" +
//                "<h1>{userNm}</h1><div>Please confirm your account by <a href='fakeLink'>clicking here</a>.</div>";