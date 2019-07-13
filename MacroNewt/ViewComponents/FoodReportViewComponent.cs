using MacroNewt.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class FoodReportViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Food food)
        {
            return View(food);
        }
    }
}