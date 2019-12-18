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
    public class HomeController : Controller
    {
        private readonly UserManager<MacroNewtUser> _userManager;
        private readonly MacroNewtContext _context;
        private readonly SignInManager<MacroNewtUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public HomeController(UserManager<MacroNewtUser> userManager, MacroNewtContext context, 
                                SignInManager<MacroNewtUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }


        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Title = "MacroNewt Home";

            UserStatsHandler ush = new UserStatsHandler(_userManager, _context);

            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var user = User;

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

            //await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
            //            $"<h1>{userNm}</h1><div>Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.</div>");

            return ViewComponent("ResendConfirmationEmail");
        }

        //public string BuildVerificationEmailHtml(string userName, string userEmail, string callbackUrl)
        //{
        //    string emailHtml = System.IO.File.ReadAllText("Views/Shared/VerificationEmail.cshtml");

        //    StringBuilder sb = new StringBuilder(emailHtml);

        //    sb.Replace("{userName}", userName);
        //    sb.Replace("{userEmail}", userEmail);
        //    sb.Replace("{callbackUrl}", callbackUrl);

        //    string emailHtmlResult = sb.ToString();

        //    return emailHtmlResult;
        //}

        //public async Task<IActionResult> SendTestEmail()
        //{
        //    string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var user = _context.Users
        //        .Where(u => u.Id == userID)
        //        .FirstOrDefault();

        //    //     var userNm = user.Name;

        //    //string path = "Views/Shared/VerificationEmail.cshtml";





        //    //            string emailHtml = "<div style ='width:350px;' ><div class='text-center'><span style ='background-color:#ebebeb; padding:10px; display:block; text-align:left; font-weight:lighter;'> Your MacroNewt email verification</span>"
        //    //                + "<img src = 'cid:LogoImage' alt='siteLogo' title='Logo' style='max-width:150px; width:100%; margin:10px;' /></div><div style = 'padding:10px;' >< p >" + ${user.Name} + ",<br />
        //    //            Your MacroNewt account email is:<br />
        //    //            ${user.Email
        //    //}
        //    //        </p>
        //    //        <div style = "margin:10px 0 10px 0;" >
        //    //            < a class="btn btn-sm btn-myBlueDarker" id="confirmEmailButton" href="${HtmlEncoder.Default.Encode(callbackUrl)}">Confirm email</a>
        //    //         </div>
        //    //        <p>
        //    //            <br />

        //    //             This link will expire in 3 hours.
        //    //             <br />
        //    //        </p>
        //    //        <p>

        //    //             Can't click the button? Copy and past this into your browser:<br />
        //    //            ${ HtmlEncoder.Default.Encode(callbackUrl)}
        //    //        </p>
        //    //    </div>
        //    //</div>"

        //    EmailBuildHandler ebh = new EmailBuildHandler();

        //    //string emailContent = GetVerificationEmailHtml(user.Name, user.Email, HtmlEncoder.Default.Encode(callbackUrl)).ToString();
        //    //string emailContent = GetVerificationEmailHtml(user.Name, user.Email, "callbackUrlPlaceholder").ToString();

        //    string finalEmail = ebh.BuildVerificationEmailHtml(user.Name, user.Email, "placeholderCallbackUrl");

        //    await _emailSender.SendEmailAsync(user.Email, "Testing inline image", finalEmail);

        //    //  await _emailSender.SendEmailAsync(user.Email, "Testing inline image",
        //    //     $"<div><img src='cid:LogoImage' alt='siteLogo' title='Logo' style='max-width:300px; width:100%' /></div><p class='text-center'>{userNm},<p><div>Please confirm your account by <a href='fakeLink'>clicking here</a>.</div>");

        //    return ViewComponent("ResendConfirmationEmail");
        //}

        public IActionResult RefreshUserInfo()
        {
            return ViewComponent("LoggedInUserInfo");
        }

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

        //public IActionResult UpdateCalorieTargetAdjustment(int adjustment)
        //{
        //    string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var targetUser = _context.Users
        //        .Where(u => u.Id == userID)
        //        .FirstOrDefault();

        //    targetUser.DailyTargetCalories += adjustment;

        //    _context.Users.Update(targetUser);

        //    _context.SaveChanges();

        //    return Json(new { success = true });
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return ViewComponent("ContactUs", new ContactUsViewModel());
        }

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

                await _emailSender.SendEmailAsync(form.UserEmail, "User message", finalEmail);

                return ViewComponent("ConfirmContact");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
