using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MacroNewt.Models;
using Microsoft.AspNetCore.Authorization;
using MacroNewt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using MacroNewt.Models.LogicModels;
using System.Security.Claims;
using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace MacroNewt.Controllers
{
    /* 
     *  The Home controller
     *  Handles the bulk of retrieving user account details/stats and returns appropriate views
     */

    /// <summary>
    /// Controller that updates and displays proper user details/stats
    /// </summary>
    /// <seealso cref="Controller"/>
    public class HomeController : Controller
    {
        private readonly UserManager<MacroNewtUser> _userManager;
        private readonly MacroNewtContext _context;
        private readonly SignInManager<MacroNewtUser> _signInManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        /// <param name="signInManager"></param>
        /// <param name="emailSender"></param>
        public HomeController(UserManager<MacroNewtUser> userManager, MacroNewtContext context, 
                                SignInManager<MacroNewtUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Returns the main homepage view, unless user is admin in which case returns admin homepage view
        /// </summary>
        /// <returns>The Views/Home/Index <see cref="ViewResult"/></returns>
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Title = "MacroNewt Home";

            UserStatsHandler ush = new UserStatsHandler(_userManager, _context);

            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            UserConfirmedStatusViewModel ucs = new UserConfirmedStatusViewModel {
                UserEmailConfirmed = _context.Users
                    .Where(u => u.Id == userID)
                    .Select(u => u.EmailConfirmed)
                    .FirstOrDefault()
            };

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            if (!User.IsInRole("Admin"))
            {
                ush.UpdateDailyCalories(User.FindFirstValue(ClaimTypes.NameIdentifier), DateTime.Today);
            }
            
            return View(ucs);
        }

        /// <summary>
        /// Retrieves user's analytical stats and returns the graphical view component
        /// </summary>
        /// <returns>Theh MealAnalytics ViewComponent</returns>
        public IActionResult GetMealAnalytics()
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dailyTotals = _context.DailyCalTotal
                .Where(d => (d.Id == userID) && (DateTime.Now - d.CalorieDay).TotalDays <= 31)
                .ToList();

            MealAnalyticsViewModel mav = new MealAnalyticsViewModel
            {
                DailyTotals = dailyTotals
            };

            foreach (DailyCalTotal d in dailyTotals)
            {
                mav.FatMonthTotal += d.TotalDailyFatCalories;
                mav.CarbMonthTotal += d.TotalDailyCarbCalories;
                mav.ProteinMonthTotal += d.TotalDailyProteinCalories;
            }

            return ViewComponent("MealAnalytics", mav);
        }

        /// <summary>
        /// Determines if user account includes required details for finding BMR. Returns view component
        ///     for providing those details if not present, otherwise returns BMR calculator view component
        /// </summary>
        /// <returns>The UpdateProfile ViewComponent if necessary, otherwise the BMRCalculator ViewComponent</returns>
        public IActionResult CheckProfileComplete()
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _context.Users
                .Where(u => u.Id == userID)
                .FirstOrDefault();

            if ((user.HeightFeet == -1) || (user.HeightInches == -1) || (user.Weight == 0) || (user.Gender == null))
            {
                UpdateProfileViewModel upvm = new UpdateProfileViewModel
                {
                    Gender = user.Gender,
                    HeightFeet = user.HeightFeet,
                    HeightInches = user.HeightInches,
                    Weight = user.Weight
                };

                return ViewComponent("UpdateProfile", upvm);
            }

            int height = (user.HeightFeet * 12) + (user.HeightInches);

            BMRCalculatorViewModel bmrc = new BMRCalculatorViewModel(
                user.Weight,
                height,
                user.Age,
                user.Gender
            );

            return ViewComponent("BMRCalculator", bmrc);
        }

        /// <summary>
        /// Handles user request to resend confirmation email if lost or not received
        /// </summary>
        /// <returns>The ResendConfirmationEmail ViewComponent</returns>
        public async Task<IActionResult> ResendConfirmationEmail()
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _context.Users
                .Where(u => u.Id == userID)
                .FirstOrDefault();

            var userNm = user.UserName;

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code },
                protocol: Request.Scheme);

            EmailBuildHandler ebh = new EmailBuildHandler();

            string finalEmail = ebh.BuildVerificationEmailHtml(user.Name, user.Email, callbackUrl);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", finalEmail);

            return ViewComponent("ResendConfirmationEmail");
        }

        /// <summary>
        /// Refreshes stats for the logged in user after changes are made (i.e. logging new meals)
        /// </summary>
        /// <returns>The LoggedInUserInfo ViewComponent</returns>
        public IActionResult RefreshUserInfo()
        {
            return ViewComponent("LoggedInUserInfo");
        }

        /// <summary>
        /// Updates <see cref="MacroNewtUser"/> account with details necessary for BMR calculation
        /// </summary>
        /// <param name="form"></param>
        /// <returns>The BMRCalculator ViewComponent</returns>
        public IActionResult UpdateProfileStats([Bind("Gender,HeightFeet,HeightInches,Weight")] UpdateProfileViewModel form)
        {

            if (!ModelState.IsValid)
            {
                return ViewComponent("UpdateProfile", form);
            }

            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _context.Users
                .Where(u => u.Id == userID)
                .FirstOrDefault();

            user.Gender = form.Gender;
            user.HeightFeet = form.HeightFeet;
            user.HeightInches = form.HeightInches;
            user.Weight = form.Weight;

            _context.Users.Update(user);

            _context.SaveChanges();


            int height = (user.HeightFeet * 12) + (user.HeightInches);

            BMRCalculatorViewModel bmrc = new BMRCalculatorViewModel(
                user.Weight,
                height,
                user.Age,
                user.Gender
            );

            return ViewComponent("BMRCalculator", bmrc);
            
        }

        /// <summary>
        /// Updates the <see cref="UserGoals"/> target calorie goal based on chosen criteria
        /// </summary>
        /// <param name="calTarget"></param>
        /// <returns>A Json object representing a successful update of user account</returns>
        public IActionResult UpdateCalorieTarget(int calTarget)
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int calAdjustment = 0;

            var targetUser = _context.Users
                .Where(u => u.Id == userID)
                .FirstOrDefault();

            var targetUserGoals = _context.UserGoals
                .Where(g => g.Id == targetUser.Id)
                .FirstOrDefault();

            if (targetUserGoals != null)
            {
                targetUserGoals.BaseCalorieTarget = calTarget;

                _context.UserGoals.Update(targetUserGoals);

                switch (targetUserGoals.OverallGoal)
                {
                    case "Loss":
                        switch (targetUserGoals.Pace)
                        {
                            case "Relaxed":
                                calAdjustment = -250;
                                break;
                            case "Neutral":
                                calAdjustment = -500;
                                break;
                            case "Aggressive":
                                calAdjustment = -1000;
                                break;
                        }
                        break;
                    case "Gain":
                        switch (targetUserGoals.Pace)
                        {
                            case "Relaxed":
                                calAdjustment = 250;
                                break;
                            case "Neutral":
                                calAdjustment = 500;
                                break;
                            case "Aggressive":
                                calAdjustment = 1000;
                                break;
                        }
                        break;
                }

                targetUserGoals.CalAdjustment = calAdjustment;

                _context.UserGoals.Update(targetUserGoals);
            }


            targetUser.DailyTargetCalories = calTarget + calAdjustment;

            _context.Users.Update(targetUser);

            _context.SaveChanges();

            return Json(new { success = true });
        }

        /// <summary>
        /// Returns a view with some limited privacy information
        /// </summary>
        /// <returns>The Views/Home/Privacy <see cref="ViewResult"/></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Returns a view with some limited information about <see cref="MacroNewt"/>
        /// </summary>
        /// <returns>The Views/Home/About <see cref="ViewResult"/></returns>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Returns a view component enabling the user to contact admin with questions/comments
        /// </summary>
        /// <returns>The ContactUs ViewComponent</returns>
        public IActionResult Contact()
        {
            return ViewComponent("ContactUs", new ContactUsViewModel());
        }
        
        /// <summary>
        /// Uses the <see cref="EmailBuildHandler"/> and <see cref="EmailSender"/> to submit user's question/comment email
        /// </summary>
        /// <remarks>
        /// User provides a message along with a contact type meant only for organization as the type doesn't change anything about the process
        /// </remarks>
        /// <param name="form"></param>
        /// <returns>The ConfirmContact ViewComponent</returns>
        public async Task<IActionResult> ConfirmContact([Bind("UserEmail,ContactType,Message")] ContactUsViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return ViewComponent("ContactUs", form);
            }
            else
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var targetUser = _context.Users
                    .Where(u => u.Id == userID)
                    .FirstOrDefault();

                EmailBuildHandler ebh = new EmailBuildHandler();

                string finalEmail = ebh.BuildContactUsEmailHtml(targetUser.Name, form.UserEmail, form.ContactType, form.Message);

                string contentString = "User " + form.ContactType;

                await _emailSender.SendEmailAsync(form.UserEmail, contentString, finalEmail);

                return ViewComponent("ConfirmContact");
            }
        }

        /// <summary>
        /// Returns a customized error view for when something goes wrong
        /// </summary>
        /// <returns>The Views/Shared/Error <see cref="ViewResult"/></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
