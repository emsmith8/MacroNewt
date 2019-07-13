using MacroNewt.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class NoMeasureFoodNutritionLabelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(NoMeasureFood nmf)
        {
            return View(nmf);
        }
    }
}
