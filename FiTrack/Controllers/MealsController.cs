using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FiTrack.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using FiTrack.Models.ViewModels;
using Newtonsoft.Json.Linq;
using FiTrack.Models.Type;
using FiTrack.Models.LogicModels;

namespace FiTrack.Controllers
{
    public class MealsController : Controller
    {
        private readonly FiTrackContext _context;

        public MealsController(FiTrackContext context)
        {
            _context = context;
        }
   

        [HttpPost]
        public IActionResult FoodViewModel(string formData)
        {
            List<FoodListItem> foodItems = JsonConvert.DeserializeObject<List<FoodListItem>>(formData);

            SearchHandler handler = new SearchHandler();

            var client = HttpClientAccessor.HttpClient;

            string dataObjects = "";
   //         List<String> dataObjects = new List<String>();

            foreach (FoodListItem f in foodItems)
            {
                HttpResponseMessage response = client.GetAsync(handler.OrganizeReportQ(f.Ndbno)).Result;

                if (response.IsSuccessStatusCode)
                {

                    dataObjects = response.Content.ReadAsStringAsync().Result;

                    foodItems = handler.AddNutrientValue(dataObjects, foodItems);

     //               dataObjects.Add(response.Content.ReadAsStringAsync().Result);

    //                        return View(handler.StoreReportReturns(dataObjects, foodItems));

                }
                else
                {
                    Debug.WriteLine("Something went wrong with the request I guess");
                    return View();
                }

                
            }

            foreach (FoodListItem fli in foodItems)
            {
                Debug.WriteLine("TALIWEJR;ASDJF;LAKSFJ " + fli.Value);
            }

            FoodViewModel vm = new FoodViewModel()
            {
                Foods = foodItems
            };

            return View(vm);

 //           return View(handler.StoreReportReturns(dataObjects, foodItems));
            /*
                        FoodViewModel vm = new FoodViewModel(){
                            Foods = foodItems
                        };

                        return View(vm);
              */
        }
        


        // GET: Meals
        public async Task<IActionResult> Index()
        {
            return View(await _context.Meal.ToListAsync());
        }
        

        // GET: Meals/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(meal);
        }

        // GET: Meals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Meals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,MealDate,Calories")] Meal meal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(meal);
        }

        // GET: Meals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal.FindAsync(id);
            if (meal == null)
            {
                return NotFound();
            }
            return View(meal);
        }

        // POST: Meals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,MealDate,Calories")] Meal meal)
        {
            if (id != meal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealExists(meal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(meal);
        }

        // GET: Meals/Delete/5
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

            return View(meal);
        }

        // POST: Meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meal = await _context.Meal.FindAsync(id);
            _context.Meal.Remove(meal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MealExists(int id)
        {
            return _context.Meal.Any(e => e.Id == id);
        }
    }
}
