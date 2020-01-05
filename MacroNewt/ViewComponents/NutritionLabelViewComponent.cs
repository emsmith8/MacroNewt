using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            Food food = Context.Food
                .Include(x => x.Nutrients)
                    .ThenInclude(x => x.Measures)
                .FirstOrDefault(x => x.FoodId == foodId);
            return View(food);
        }
    }
}
