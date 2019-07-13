using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MacroNewt.Models;
using MacroNewt.Models.LogicModels;
using MacroNewt.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MacroNewt.Areas.Identity.Data
{
    public class LoggerController : Controller
    {
        private MacroNewtContext _context;
        private readonly UserManager<MacroNewtUser> _userManager;

        public LoggerController(MacroNewtContext context, UserManager<MacroNewtUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Title = "Log Meal";
            return View();
        }
        
        public IActionResult SearchFoods(string foodName, string targetDatabase)
        {
            Debug.WriteLine("Well, I got here and the foodName is " + foodName);
            SearchHandler handler = new SearchHandler();

            var client = HttpClientAccessor.HttpClient;

            HttpResponseMessage response = client.GetAsync(handler.OrganizeSearchQ(foodName, targetDatabase)).Result;

            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;

                JObject responseStatus = JObject.Parse(dataObjects);

                if (responseStatus["errors"] != null)
                {
                    Debug.WriteLine("Something went wrong with the request I guess");
                    return new ContentResult
                    {
                        ContentType = "text/html",
                        Content = $"<strong style='color:red'>Couldn't find '{foodName}'. Please try again.</strong>"
                    };
                }

                return ViewComponent("FoodSearchResult", handler.StoreSearchReturns(dataObjects));
                
            }
            else
            {
                Debug.WriteLine("Something went wrong with the request I guess");
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = $"<strong style='color:red'>Couldn't find '{foodName}'. Please try again.</strong>"
                };
            }
        }
        

        public IActionResult GetMealDetailViewComponent(string formData, int mId, bool reLogged)
        {
            Debug.WriteLine("The mId is " + mId);

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

        public IActionResult GetMealReviewViewComponent(string mealTitle, int mealID, string edited)
        {

            Debug.WriteLine("****************************I'm here *********************************************");

            Debug.WriteLine("WELL AT LEAST I GOT HERE and the mealTitle is " + mealTitle);
            

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
                   //handle failure
                }
                
            }

            FoodViewModel fvm = new FoodViewModel()
            {
                Foods = foodItems
            };

            return fvm;

        }
        

        public IActionResult ConfirmMeal([Bind("CalorieTotal,ProteinTotal,FatTotal,CarbTotal,Title,MealDate,MealType,Foods,MealId,UserId,UserEmail,ReLogged")] FoodViewModel form)
        {
            Debug.WriteLine("The MealId is " + form.MealId);

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
                Debug.WriteLine("I'm here and i'm adding ndbno " + f.Ndbno);
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