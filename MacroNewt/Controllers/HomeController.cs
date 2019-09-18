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

namespace MacroNewt.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<MacroNewtUser> _userManager;
        private readonly MacroNewtContext _context;
        private readonly SignInManager<MacroNewtUser> _signInManager;

        public HomeController(UserManager<MacroNewtUser> userManager, MacroNewtContext context, SignInManager<MacroNewtUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }


        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Title = "MacroNewt Home";

            UserStatsHandler ush = new UserStatsHandler(_userManager, _context);

            var user = User;
            
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            if (!User.IsInRole("Admin"))
            {
                ush.UpdateDailyCalories(User.FindFirstValue(ClaimTypes.NameIdentifier), DateTime.Today);
            }
            
            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
