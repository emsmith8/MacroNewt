using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class ConfirmContact : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
