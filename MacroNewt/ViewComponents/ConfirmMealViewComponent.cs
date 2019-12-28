using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class ConfirmMealViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(ConfirmMealViewModel cmvm)
        {
            return View(cmvm);
        }
    }
}
