using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MacroNewt.ViewComponents
{
    public class OldMealsModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<MacroNewt.Models.Meal> oldMeals)
        {
            return View(oldMeals);
        }
    }
}
