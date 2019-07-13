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
        //private MacroNewtContext _context { get; set; }

        //public ConfirmMealViewComponent(MacroNewtContext context)
        //{
        //    _context = context;
        //}

        public IViewComponentResult Invoke(ConfirmMealViewModel cmvm)
        {
            return View(cmvm);
        }
    }
}
