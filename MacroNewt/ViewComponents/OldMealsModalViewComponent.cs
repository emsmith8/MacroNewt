using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
