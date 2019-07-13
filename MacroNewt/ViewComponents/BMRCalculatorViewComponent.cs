using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class BMRCalculatorViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(BMRCalculatorViewModel bmrc)
        {
            return View(bmrc);
        }
    }
}
