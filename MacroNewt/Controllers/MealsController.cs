using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MacroNewt.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using MacroNewt.Models.ViewModels;
using Newtonsoft.Json.Linq;
using MacroNewt.Models.LogicModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MacroNewt.Areas.Identity.Data
{
    public class MealsController : Controller
    {
        private readonly MacroNewtContext _context;
        private readonly UserManager<MacroNewtUser> _userManager;

        public MealsController(MacroNewtContext context, UserManager<MacroNewtUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Meal History";

            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            /* CAN GET RID OF */
            HistoryDataViewModel hdv = new HistoryDataViewModel
            {
                Meals = await _context.Meal
                    .Where(m => m.UserId == userID)
                    .ToListAsync(),
                DailyTotals = await _context.DailyCalTotal
                    .Where(d => d.Id == userID)
                    .ToListAsync()
            };


            return View(hdv);

            //return View(await _context.Meal
            //    .Where(m => m.UserId == userID)
            //    .ToListAsync());
        }

        [Authorize]
        public IActionResult Explore()
        {
            ViewBag.Title = "Explore Foods";
            return View();
        }

        //public IActionResult BuildExplorePortionModal()
        //{

        //}

        public IActionResult BuildExploreNutritionLabelModal(string ndbno, int portionIndex)
        {
            ExplorePortionChoiceViewModel epc = new ExplorePortionChoiceViewModel
            {
                Ndbno = ndbno,
                PortionIndex = portionIndex
            };

            return ViewComponent("ExploreNutritionLabel", epc);
        }

        public IActionResult BuildMealCalendarModal()
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var mealList = _context.Meal
                    .Where(m => m.UserId == userID)
                    .ToList();

            
            
            //foreach (Meal meal in mealList)
            //{
            //    string mealTitle = meal.Title;
            //    DateTime mealDate = meal.MealDate.Date;
            //    int mealCalories = meal.Calories;
            //}

            return ViewComponent("MealCalendarHistory", mealList);
        }



        public IActionResult GetOldMealsModal()
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var meals = _context.Meal
                .Where(m => m.UserId == userID)
                .ToList();

            return ViewComponent("OldMealsModal", meals);
        }

        public IActionResult GetCurrentDayCalStats()
        {
            UserStatsHandler ush = new UserStatsHandler(_userManager, _context);

            MacroNewtUser user = _userManager.GetUserAsync(HttpContext.User).Result;

            return ViewComponent("CurrentDayCalStats", ush.GetCurrentMacroTargets(user));
        }

        public IActionResult GetSpecificDayCalStats(string targetDate)
        {
            UserStatsHandler ush = new UserStatsHandler(_userManager, _context);

            MacroNewtUser user = _userManager.GetUserAsync(HttpContext.User).Result;

            return ViewComponent("PastDayCalStats", ush.GetPastMacroTargets(user, targetDate));
        }
        

        public IActionResult GetDateRangeHistoryViewComponent(DateTime startDate, DateTime endDate)
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Meal> meals = new List<Meal>();

            if (startDate.Date == endDate.Date)
            {
                meals = _context.Meal
                    .Where(m => (m.UserId == userID) && (m.MealDate.Date == startDate.Date))
                    .ToList();
            }
            else
            {
                meals = _context.Meal
                    .Where(m => (m.UserId == userID) && ((m.MealDate.Date >= startDate.Date) && (m.MealDate.Date <= endDate.Date)))
                    .ToList();
            }

            HistoryDataViewModel hd = new HistoryDataViewModel()
            {
                Meals = meals,
                StartDate = startDate,
                EndDate = endDate
            };

            return ViewComponent("DateRangeHistory", hd);
            
        }
        

        public IActionResult GetNutritionLabelViewComponent(string ndbno)
        {
            var food = _context.Food
                .Include(x => x.Nutrients)
                    .ThenInclude(x => x.Measures)
                    .FirstOrDefault(f => f.Ndbno == ndbno);

            if (food == null)
            {
                return NotFound();
            }

            return View();
        }

        // GET: Meals/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal
                .Include(x => x.FoodComponents)
                    .ThenInclude(x => x.Nutrients)
                    .ThenInclude(x => x.Measures)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (meal == null)
            {
                return NotFound();
            }

            var mealCount = _context.Meal
                .Where(x => (x.MealDate.Date == meal.MealDate.Date) && (x.UserId == meal.UserId)).Count();

            var userEmail = _context.Users
                .Where(u => u.Id == meal.UserId)
                .Select(u => u.Email)
                .FirstOrDefault();

            FoodViewModel fvm = new FoodViewModel()
            {
                Title = meal.Title,
                MealDate = meal.MealDate,
                Foods = meal.FoodComponents,
                CalorieTotal = meal.Calories,
                MealCount = mealCount,
                MealId = meal.Id,
                UserEmail = userEmail
            };

            return View(fvm);
        }

        // GET: Meals/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id, bool relog = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal
                .Include(m => m.FoodComponents)
                    .ThenInclude(m => m.Nutrients)
                    .ThenInclude(m => m.Measures)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (meal == null)
            {
                return NotFound();
            }

            if (relog)
            {
                meal.ReLogged = true;
                meal.MealDate = DateTime.Now;
                meal.Title = null;
            }

            return View(meal);
        }

        // GET: Meals/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            return ViewComponent("DeleteMealModal", meal);
        }

        // POST: Meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UserStatsHandler ush = new UserStatsHandler(_userManager, _context);
            
            var meal = await _context.Meal
                .FirstOrDefaultAsync(m => m.Id == id);

            var date = meal.MealDate;

            var userId = await _context.Users
                .Where(u => u.Id == meal.UserId)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            _context.Meal.Remove(meal);
            await _context.SaveChangesAsync();
            
            ush.UpdateDailyCalories(userId, date);

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("ManageMeals", "Admin");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MealExists(int id)
        {
            return _context.Meal.Any(e => e.Id == id);
        }

        public async Task<JsonResult> GetMonthMealStatus(int month, int year)
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Dictionary<string,object> results = new Dictionary<string, object>();

            var subResults = new List<object>();

            var dt = await _context.DailyCalTotal
                    .Where(d => (d.Id == userID) && (d.CalorieDay.Month == (month+1)) && (d.CalorieDay.Year == year) && (d.TotalDailyCalories != 0))
                    .ToListAsync();

            string overUnder = "";

            for (int i = 0; i < dt.Count; i++)
            {
                if (dt[i].TotalDailyCalories > dt[i].TargetDailyCalories)
                {
                    overUnder = "over";
                }
                else
                {
                    overUnder = "under";
                }

                subResults.Add(new { Status = overUnder, Total = dt[i].TotalDailyCalories });

                results.Add(dt[i].CalorieDay.ToString("MM/dd/yy"), subResults[i]);
            }

            return Json(results);
        }

        public async Task<IActionResult> GetDayMealInfo(int month, int day, int year)
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dayInfo = await _context.Meal
                .Where(m => (m.UserId == userID) && (m.MealDate.Month == month) && (m.MealDate.Day == day) && (m.MealDate.Year == year))
                .ToListAsync();

            return ViewComponent("CalendarMealModal", dayInfo);
        }
    }
}
