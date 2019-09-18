using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class MealAnalyticsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MealAnalyticsViewModel mav)
        {
            return View(mav);
        }
    }
}
