using MacroNewt.Models;
using MacroNewt.Models.LogicModels;
using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace MacroNewt.Areas.Identity.Data
{
    /*
     *  The Logger controller
     *  Handles user API food searches and meal logging. Meal logging is accomplished within one view for data persistence,
     *  making use of multiple view components injected into a smartwizard step plugin
     */

    /// <summary>
    /// Controller that handles the food searching and meal logging process.
    /// </summary>
    /// <seealso cref="Controller"/>
    public class LoggerController : Controller
    {
        private MacroNewtContext _context;
        private readonly UserManager<MacroNewtUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerController"/> class.
        /// </summary>
        /// <param name="context">The dependency-injected <see cref="MacroNewtContext"/> obtained from the <see cref="Startup.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)"/></param>
        /// <param name="userManager">The dependency-injected <see cref="UserManager{TUser}"/></param>
        public LoggerController(MacroNewtContext context, UserManager<MacroNewtUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns the meal logging view, which starts with food searching. Restricted access for only confirmed accounts
        /// </summary>
        /// <returns>The Views/Logger/Index <see cref="ViewResult"/></returns>
        [Authorize]
        public IActionResult Index()
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userEmailStatus = _context.Users
                .Where(u => u.Id == userID)
                .Select(u => u.EmailConfirmed)
                .FirstOrDefault();

            if (!userEmailStatus)
            {
                return RedirectToAction("NotConfirmed", "Logger");
            }

            ViewBag.Title = "Log Meal";
            return View();
        }

        /// <summary>
        /// Returns a view notifying user he/she has not confirmed his/her account and cannot log meals until confirmed
        /// </summary>
        /// <returns>The Views/Logger/NotConfirmed <see cref="ViewResult"/></returns>
        public IActionResult NotConfirmed()
        {
            return View();
        }

        /// <summary>
        /// Sends a request to the USDA food search API for user specified input in either branded or unbranded database.
        ///     Results are displayed in a view component below search area
        /// </summary>
        /// <remarks>
        /// Makes use of the <see cref="SearchHandler"/> class for organizing API requests
        /// </remarks>
        /// <param name="foodName">The string input name used during the food search</param>
        /// <param name="targetDatabase">The user-chosen database to be searched, either branded or unbranded</param>
        /// <returns>The FoodSearchResult ViewComponent if search returns results - a message otherwise</returns>
        public IActionResult SearchFoods(string foodName, string targetDatabase)
        {
            SearchHandler handler = new SearchHandler();

            var client = HttpClientAccessor.HttpClient;

            HttpResponseMessage response = client.GetAsync(handler.OrganizeSearchQ(foodName, targetDatabase)).Result;

            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;

                JObject responseStatus = JObject.Parse(dataObjects);

                if (responseStatus["errors"] != null)
                {
                    return new ContentResult
                    {
                        ContentType = "text/html",
                        Content = $"<strong style='color:red'>Couldn't find '{foodName}'. Please try again.</strong>"
                    };
                }
                /* Thread sleep used for mimicking slow response time in deployed environment, mainly for testing wait spinner modals */
                //System.Threading.Thread.Sleep(3000);
                return ViewComponent("FoodSearchResult", handler.StoreSearchReturns(dataObjects));

            }
            else
            {
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = $"<strong style='color:red'>Couldn't find '{foodName}'. Please try again.</strong>"
                };
            }
        }

        /// <summary>
        /// Organizes the details of all selected <see cref="Food"/> items and returns a view component prompting 
        ///     for details to be completed for <see cref="Meal"/> logging purposes
        /// </summary>
        /// <remarks>
        /// Makes use of the <see cref="SearchHandler"/> class for organizing API request and storing response data
        /// </remarks>
        /// <param name="formData">A string containing nutritional information of all foods included in meal</param>
        /// <param name="mId">An int identifier of the current meal</param>
        /// <param name="reLogged">An indicator of whether the meal is being re-logged, providing an easier user-process if so</param>
        /// <returns>The MealDetail ViewComponent</returns>
        public IActionResult GetMealDetailViewComponent(string formData, int mId, bool reLogged)
        {
            List<Food> foodItems = JsonConvert.DeserializeObject<List<Food>>(formData);

            SearchHandler handler = new SearchHandler();

            var client = HttpClientAccessor.HttpClient;

            for (int i = 0; i < foodItems.Count; i++)
            {
                HttpResponseMessage response = client.GetAsync(handler.OrganizeReportQ(foodItems[i].Ndbno)).Result;

                if (response.IsSuccessStatusCode)
                {

                    JObject dataObject = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                    foodItems[i] = handler.StoreMealNutrientDetails(foodItems[i], dataObject, "simple");

                }
                else
                {
                    // handle failure
                }
            }

            FoodViewModel vm = new FoodViewModel()
            {
                Foods = foodItems,
                MealDate = DateTime.Now,
                MealId = mId
            };

            if (mId != 0)
            {

                vm.UserId = _context.Meal
                    .Where(m => m.Id == mId)
                    .Select(m => m.UserId)
                    .FirstOrDefault();

                vm.Title = _context.Meal
                    .Where(m => m.Id == mId)
                    .Select(m => m.Title)
                    .FirstOrDefault();

                vm.MealDate = _context.Meal
                    .Where(m => m.Id == mId)
                    .Select(m => m.MealDate)
                    .FirstOrDefault();

                vm.Edited = "edited";

                vm.MealType = _context.Meal
                    .Where(m => m.Id == mId)
                    .Select(m => m.MealType)
                    .FirstOrDefault();


                if (vm.UserId == _userManager.GetUserId(User))
                {
                    vm.UserEmail = null;
                }
                else
                {
                    vm.UserEmail = _context.Users
                    .Where(u => u.Id == vm.UserId)
                    .Select(u => u.Email)
                    .FirstOrDefault();
                }

            }

            if (reLogged)
            {
                vm.ReLogged = "true";
                vm.MealId = 0;
                vm.Title = null;
                vm.MealDate = DateTime.Now;
                vm.MealType = null;
            }

            return ViewComponent("MealDetail", vm);
        }

        /// <summary>
        /// Returns a <see cref="Meal"/> summary view component after meal logging is complete
        /// </summary>
        /// <param name="mealTitle">The string name of the meal logged by user</param>
        /// <param name="mealID">The int identifier of the meal logged by user</param>
        /// <param name="edited">A string indicator of whether the meal has been edited rather than newly created</param>
        /// <returns>The MealReview ViewComponent</returns>
        public IActionResult GetMealReviewViewComponent(string mealTitle, int mealID, string edited)
        {

            if (mealTitle == null)
            {
                return NotFound();
            }

            var meal = new Meal();

            meal = _context.Meal
            .Include(x => x.FoodComponents)
                .ThenInclude(x => x.Nutrients)
                .ThenInclude(x => x.Measures)
            .FirstOrDefault(m => m.Id == mealID);

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
                MealType = meal.MealType,
                Foods = meal.FoodComponents,
                CalorieTotal = meal.Calories,
                MealCount = mealCount,
                UserEmail = userEmail,
                Edited = edited
            };

            return ViewComponent("MealReview", fvm);

        }

        /// <summary>
        /// Handles the retrieval of <see cref="Nutrient"/> data for all selected food items in the meal to be logged
        /// </summary>
        /// <remarks>
        /// Makes use of the <see cref="SearchHandler"/> class for organizing API request and storing nutrient data.
        ///     Nutrient data isn't stored originally because food search API responses don't include these details
        /// </remarks>
        /// <param name="ndbnos">A list of all food identifiers to be included in nutritional data API queries</param>
        /// <returns>A populated <see cref="FoodViewModel"/></returns>
        public FoodViewModel AddNutrientData(List<string> ndbnos)
        {
            SearchHandler handler = new SearchHandler();

            var client = HttpClientAccessor.HttpClient;

            List<Food> foodItems = new List<Food>();

            foreach (string n in ndbnos)
            {
                HttpResponseMessage response = client.GetAsync(handler.OrganizeReportQ(n)).Result;

                if (response.IsSuccessStatusCode)
                {
                    JObject dataObject = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                    Food f = new Food();

                    foodItems.Add(handler.StoreMealNutrientDetails(f, dataObject, "full"));

                }
                else
                {
                    // handle failure                    
                }

            }

            FoodViewModel fvm = new FoodViewModel()
            {
                Foods = foodItems
            };

            return fvm;

        }

        /// <summary>
        /// Checks ModelState and returns same view if invalid, otherwise provides user with view component for confirming meal to be logged
        /// </summary>
        /// <param name="form">A <see cref="FoodViewModel"/> object populated with meal data to be shown before confirming the meal</param>
        /// <returns>The ConfirmMeal ViewComponent</returns>
        public IActionResult ConfirmMeal([Bind("CalorieTotal,ProteinTotal,FatTotal,CarbTotal,Title,MealDate,MealType,Foods,MealId,UserId,UserEmail,ReLogged")] FoodViewModel form)
        {
            if (!ModelState.IsValid)
            {
                Response.Headers.Add("vstatus", "fail");

                return ViewComponent("MealDetail", form);
            }
            else
            {
                UserStatsHandler ush = new UserStatsHandler(_userManager, _context);

                MacroNewtUser user;

                if (form.UserId == null)
                {
                    user = _userManager.GetUserAsync(HttpContext.User).Result;
                }
                else
                {
                    user = _context.Users.FirstOrDefault(u => u.Id == form.UserId);
                }


                Response.Headers.Add("vstatus", "pass");

                Boolean admin = false;

                if (User.IsInRole("Admin"))
                {
                    admin = true;
                }

                return ViewComponent("ConfirmMeal", ush.GetMacroTargets(user, admin, form.MealId, form.MealDate, form.CalorieTotal, form.ProteinTotal, form.FatTotal, form.CarbTotal));
            }

        }

        /// <summary>
        /// Creates or edits a meal in the database according to the provided foods and meal details
        /// </summary>
        /// <remarks>
        /// Also updates user's daily stats with use of <see cref="UserStatsHandler"/> class.
        /// </remarks>
        /// <param name="form">A <see cref="FoodViewModel"/> object populated with meal data to be logged</param>
        /// <returns>A Json object representing successful meal logging</returns>
        public IActionResult LogMeal([Bind("CalorieTotal,ProteinTotal,FatTotal,CarbTotal,Title,MealDate,MealType,Foods,MealId,UserId")] FoodViewModel form)
        {
            string UserId;

            if (form.MealId == 0)
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                UserId = form.UserId;
            }

            if (form.Title == "Auto")
            {
                var currentMealCount = _context.Meal
                    .Where(x => (x.MealDate.Date == form.MealDate.Date) && (x.UserId == UserId)).Count() + 1;

                form.Title = form.MealDate.ToString("MM/dd/yyyy") + " - Meal #" + currentMealCount.ToString();
            }

            UserStatsHandler ush = new UserStatsHandler(_userManager, _context);

            var UserEmail = _context.Users.FirstOrDefault(u => u.Id == UserId).Email;

            List<string> foodNdbnos = new List<string>();

            foreach (var f in form.Foods)
            {
                foodNdbnos.Add(f.Ndbno);
            }

            FoodViewModel fvm = AddNutrientData(foodNdbnos);

            for (int i = 0; i < form.Foods.Count; i++)
            {
                fvm.Foods[i].NumberOfServings = form.Foods[i].NumberOfServings;
                fvm.Foods[i].PortionIndex = form.Foods[i].PortionIndex;
                fvm.Foods[i].Value = form.Foods[i].Value;
                if (form.Foods[i].Nutrients[0].Measures.Count == 0)
                {
                    fvm.Foods[i].SelectedPortionLabel = "100g";
                    fvm.Foods[i].SelectedPortionQty = "1";
                }
                else
                {
                    fvm.Foods[i].SelectedPortionLabel = form.Foods[i].Nutrients[0].Measures[form.Foods[i].PortionIndex - 1].Label;
                    fvm.Foods[i].SelectedPortionQty = form.Foods[i].Nutrients[0].Measures[form.Foods[i].PortionIndex - 1].Qty;
                }

            }


            if (form.MealId == 0)
            {
                Meal m = new Meal()
                {
                    Calories = form.CalorieTotal,
                    MealDate = form.MealDate,
                    MealType = form.MealType,
                    Title = form.Title,
                    FoodComponents = fvm.Foods,
                    UserId = UserId,
                    UserEmail = UserEmail,
                    ProteinCalories = (form.ProteinTotal * 4),
                    FatCalories = (form.FatTotal * 9),
                    CarbCalories = (form.CarbTotal * 4)
                };
                _context.Meal.Add(m);

                _context.SaveChanges();

                ush.UpdateDailyCalories(UserId, m.MealDate);

                return Json(new { success = true, mealID = m.Id });
            }
            else
            {
                Meal m = new Meal()
                {
                    Calories = form.CalorieTotal,
                    MealDate = form.MealDate,
                    MealType = form.MealType,
                    Title = form.Title,
                    FoodComponents = fvm.Foods,
                    UserId = UserId,
                    UserEmail = UserEmail,
                    ProteinCalories = (form.ProteinTotal * 4),
                    FatCalories = (form.FatTotal * 9),
                    CarbCalories = (form.CarbTotal * 4)
                };

                var mealToRemove = _context.Meal
                    .Where(x => x.Id == form.MealId)
                    .FirstOrDefault();

                _context.Meal.Remove(mealToRemove);

                _context.SaveChanges();

                _context.Meal.Add(m);

                _context.SaveChanges();

                ush.UpdateDailyCalories(UserId, m.MealDate);


                return Json(new { success = true, mealID = m.Id });
            }
        }

    }
}