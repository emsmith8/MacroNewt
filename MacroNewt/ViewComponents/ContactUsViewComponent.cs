using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class ContactUsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ContactUsViewModel cuvm)
        {
            return View(cuvm);
        }
    }
}
