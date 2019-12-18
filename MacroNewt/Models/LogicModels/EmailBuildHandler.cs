using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MacroNewt.Models.LogicModels
{
    public class EmailBuildHandler
    {
        public string BuildVerificationEmailHtml(string userName, string userEmail, string callbackUrl)
        {
            string htmlContent = File.ReadAllText("wwwroot/EmailTemplates/VerificationEmail.html");

            StringBuilder sb = new StringBuilder(htmlContent);

            sb.Replace("{userName}", userName);
            sb.Replace("{userEmail}", userEmail);
            sb.Replace("{callbackUrl}", callbackUrl);

            string emailHtmlResult = sb.ToString();

            return emailHtmlResult;
        }

        public string BuildForgotPasswordEmailHtml(string userEmail, string callbackUrl)
        {
            string htmlContent = File.ReadAllText("wwwroot/EmailTemplates/ForgotPasswordEmail.html");

            StringBuilder sb = new StringBuilder(htmlContent);

            sb.Replace("{userEmail}", userEmail);
            sb.Replace("{callbackUrl}", callbackUrl);

            string emailHtmlResult = sb.ToString();

            return emailHtmlResult;
        }

        public string BuildContactUsEmailHtml(string userName, string userEmail, string contactType, string userMessage)
        {
            string htmlContent = File.ReadAllText("wwwroot/EmailTemplates/ContactUsEmail.html");

            StringBuilder sb = new StringBuilder(htmlContent);

            sb.Replace("{contactType}", contactType);
            sb.Replace("{userName}", userName);
            sb.Replace("{userEmail}", userEmail);
            sb.Replace("{userMessage}", userMessage);

            string emailHtmlResult = sb.ToString();

            return emailHtmlResult;
        }
    }
}
