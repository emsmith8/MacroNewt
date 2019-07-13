using MacroNewt.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class DeleteMealModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Meal meal)
        {
            return View(meal);
        }
    }
}
