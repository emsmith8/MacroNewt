using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
