using MacroNewt.Models;
using MacroNewt.Models.LogicModels;
using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MacroNewt.Areas.Identity.Data
{
    /*
     *  The Meals controller
     *  Handles history view of user meals and allows for editing, review, and deletion of past meals
     */

    /// <summary>
    /// Controller that displays a calendar view of previous user meals and allows for editing/review/deletion of logged meals
    /// </summary>
    /// <seealso cref="Controller"/>
    public class MealsController : Controller
    {
        private readonly MacroNewtContext _context;
        private readonly UserManager<MacroNewtUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MealsController"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public MealsController(MacroNewtContext context, UserManager<MacroNewtUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns the calendar meal history view
        /// </summary>
        /// <remarks>
        /// Days are shown in either green or red if meals were logged, depending on whether user goals were met.
        ///     Meals are queried from database when user clicks on a calendar day cell.
        /// </remarks>
        /// <returns>The Views/Meals/Index <see cref="ViewResult"/></returns>
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Title = "Meal History";

            return View();
        }

        /// <summary>
        /// Returns the food explore view allowing user to browse food details without logging anything
        /// </summary>
        /// <returns>The Views/Meals/Explore <see cref="ViewResult"/></returns>
        [Authorize]
        public IActionResult Explore()
        {
            ViewBag.Title = "Explore Foods";
            return View();
        }

        /// <summary>
        /// Handles the proper display of a nutrition label for the selected portion, or <see cref="Measure"/>, of food
        /// </summary>
        /// <param name="ndbno"></param>
        /// <param name="portionIndex"></param>
        /// <returns>The ExploreNutritionLabel ViewComponent</returns>
        public IActionResult BuildExploreNutritionLabelModal(string ndbno, int portionIndex)
        {
            ExplorePortionChoiceViewModel epc = new ExplorePortionChoiceViewModel
            {
                Ndbno = ndbno,
                PortionIndex = portionIndex
            };

            return ViewComponent("ExploreNutritionLabel", epc);
        }

        /// <summary>
        /// Returns a view component with previously logged meals so that user can easily re-log meals
        /// </summary>
        /// <returns>The OldMealsModal ViewComponent</returns>
        public IActionResult GetOldMealsModal()
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var meals = _context.Meal
                .Where(m => m.UserId == userID)
                .ToList();

            return ViewComponent("OldMealsModal", meals);
        }

        /// <summary>
        /// Retrieves and displays user's current daily stats in a view component
        /// </summary>
        /// <returns>The CurrentDayCalStats ViewComponent</returns>
        public IActionResult GetCurrentDayCalStats()
        {
            UserStatsHandler ush = new UserStatsHandler(_userManager, _context);

            MacroNewtUser user = _userManager.GetUserAsync(HttpContext.User).Result;

            return ViewComponent("CurrentDayCalStats", ush.GetCurrentMacroTargets(user));
        }

        /// <summary>
        /// Retrieves and displays daily stats for a specific past date in a view component
        /// </summary>
        /// <param name="targetDate"></param>
        /// <returns>The PastDayCalStats ViewComponent</returns>
        public IActionResult GetSpecificDayCalStats(string targetDate)
        {
            UserStatsHandler ush = new UserStatsHandler(_userManager, _context);

            MacroNewtUser user = _userManager.GetUserAsync(HttpContext.User).Result;

            return ViewComponent("PastDayCalStats", ush.GetPastMacroTargets(user, targetDate));
        }

        /// <summary>
        /// Returns the details page for a previously logged meal
        /// </summary>
        /// <remarks>
        /// User can review meal information as well as view nutrition labels for foods and the entire meal
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>The Views/Meals/Details <see cref="ViewResult"/></returns>
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

        /// <summary>
        /// Returns the edit view for a previously logged meal
        /// </summary>
        /// <remarks>
        /// The edit procedure is the same as the logging procedure but has to be tied to existing data. Some redundancy exists.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="relog"></param>
        /// <returns>The Views/Meals/Edit <see cref="ViewResult"/></returns>
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

        /// <summary>
        /// Returns a meal deletion view component
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The DeleteMealModal ViewComponent</returns>
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

        /// <summary>
        /// Confirms deletion of a previous meal and updates user stats
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A redirect to last location</returns>
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

        /// <summary>
        /// Queries database for all meals in currently shown calendar month and displays red or green if user
        ///     exceeded or met daily goals
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>A Json object containing month's meal statuses</returns>
        public async Task<JsonResult> GetMonthMealStatus(int month, int year)
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Dictionary<string, object> results = new Dictionary<string, object>();

            var subResults = new List<object>();

            var dt = await _context.DailyCalTotal
                    .Where(d => (d.Id == userID) && (d.CalorieDay.Month == (month + 1)) && (d.CalorieDay.Year == year) && (d.TotalDailyCalories != 0))
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

        /// <summary>
        /// Queries database for all meals on chosen day in the past and displays view component with that day's stats
        ///     and the options to edit, review, or delete any shown meal
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="year"></param>
        /// <returns>The CalendarMealModal ViewComponent</returns>
        public async Task<IActionResult> GetDayMealInfo(int month, int day, int year)
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dayInfo = await _context.Meal
                .Where(m => (m.UserId == userID) && (m.MealDate.Month == month) && (m.MealDate.Day == day) && (m.MealDate.Year == year))
                .OrderBy(m => m.MealDate)
                .ToListAsync();

            return ViewComponent("CalendarMealModal", dayInfo);
        }

        private bool MealExists(int id)
        {
            return _context.Meal.Any(e => e.Id == id);
        }
    }
}
