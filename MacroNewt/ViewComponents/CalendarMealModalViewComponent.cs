using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
