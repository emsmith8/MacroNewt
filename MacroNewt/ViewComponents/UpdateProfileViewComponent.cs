using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class UpdateProfileViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(UpdateProfileViewModel upvm)
        {
            return View(upvm);
        }
    }
}
