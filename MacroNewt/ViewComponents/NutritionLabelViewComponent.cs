using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MacroNewt.Models;
using Microsoft.EntityFrameworkCore;
using MacroNewt.Areas.Identity.Data;

namespace MacroNewt.ViewComponents
{
    public class NutritionLabelViewComponent : ViewComponent
    {
        private MacroNewtContext Context { get; set; }

        public NutritionLabelViewComponent(MacroNewtContext _context)
        {
            Context = _context;
        }

        public IViewComponentResult Invoke(int foodId)
        {
            Food food = Context.Foods
                .Include(x => x.Nutrients)
                    .ThenInclude(x => x.Measures)
                .FirstOrDefault(x => x.FoodId == foodId);            
            return View(food);
        }
    }
}
