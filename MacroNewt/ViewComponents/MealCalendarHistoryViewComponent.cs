using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class MealCalendarHistoryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<Models.Meal> meals)
        {
            return View(meals);
        }
    }
}
