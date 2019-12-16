using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroNewt.Models.LogicModels
{
    public class EmailBuildHandler
    {
        public string BuildVerificationEmailHtml(string userName, string userEmail, string callbackUrl)
        {
            string emailHtml = System.IO.File.ReadAllText("Views/Shared/VerificationEmail.cshtml");

            StringBuilder sb = new StringBuilder(emailHtml);

            sb.Replace("{userName}", userName);
            sb.Replace("{userEmail}", userEmail);
            sb.Replace("{callbackUrl}", callbackUrl);

            string emailHtmlResult = sb.ToString();

            return emailHtmlResult;
        }
    }
}
