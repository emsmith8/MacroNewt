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

            //string emailContentPath = "C:/Users/evanm/source/repos/MacroNewt/MacroNewt/Views/Shared/VerificationEmail.cshtml";

            //string emailHtml = File.ReadAllText(emailContentPath);

            StringBuilder sb = new StringBuilder(htmlContent);

            sb.Replace("{userName}", userName);
            sb.Replace("{userEmail}", userEmail);
            sb.Replace("{callbackUrl}", callbackUrl);

            string emailHtmlResult = sb.ToString();

            return emailHtmlResult;
        }
    }
}
