using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MacroNewt.ViewComponents
{
    public class CalendarMealModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<MacroNewt.Models.Meal> mealsList)
        {
            return View(mealsList);
        }
    }
}
