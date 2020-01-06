using System.IO;
using System.Text;

namespace MacroNewt.Models.LogicModels
{
    /*
     *  The EmailBuildHandler class
     *  Consolidates and handles email templates for communication to and from site admin
     */

    /// <summary>
    /// This class consolidates the building of each type of email sent by the app
    /// </summary>
    /// <remarks>
    /// All email templates include keywords in curly brackets in specific locations based on message type. Before the email is sent,
    ///     keywords and brackets are replaced with the pertinent data (i.e. {contactType} is replaced with 'Question')
    /// </remarks>
    /// <seealso cref="Microsoft.AspNetCore.Identity.UI.Services.IEmailSender"/>
    public class EmailBuildHandler
    {
        /// <summary>
        /// Replaces keywords with a user's name, email address, and a callback url for account verification
        /// </summary>
        /// <param name="userName">The string name of the user to which the email will be sent</param>
        /// <param name="userEmail">The string email address of target user</param>
        /// <param name="callbackUrl">The string callback URL to be linked in the email triggering account verification</param>
        /// <returns>The final string of email html content</returns>
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

        /// <summary>
        /// Replaces keywords with a user's email address and callback url for account password resetting
        /// </summary>
        /// <param name="userEmail">The string email address of target user</param>
        /// <param name="callbackUrl">The string callback URL to be linked in the email providing access to password reset</param>
        /// <returns>The final string of email html content</returns>
        public string BuildForgotPasswordEmailHtml(string userEmail, string callbackUrl)
        {
            string htmlContent = File.ReadAllText("wwwroot/EmailTemplates/ForgotPasswordEmail.html");

            StringBuilder sb = new StringBuilder(htmlContent);

            sb.Replace("{userEmail}", userEmail);
            sb.Replace("{callbackUrl}", callbackUrl);

            string emailHtmlResult = sb.ToString();

            return emailHtmlResult;
        }

        /// <summary>
        /// Replaces keywords with a user's contact type, name, email address, and message for contacting admin
        /// </summary>
        /// <param name="userName">The string name of the user contacting admin</param>
        /// <param name="userEmail">The string email address of the user contacting admin</param>
        /// <param name="contactType">The string reason for contact</param>
        /// <param name="userMessage">The user's string contact message</param>
        /// <returns>The final string of email html content</returns>
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
