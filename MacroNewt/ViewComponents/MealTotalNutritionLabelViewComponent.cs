using MacroNewt.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class MealTotalNutritionLabelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MealTotalNutrient mtn)
        {
            return View(mtn);
        }
    }
}
