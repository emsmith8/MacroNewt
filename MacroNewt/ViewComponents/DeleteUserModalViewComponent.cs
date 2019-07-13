using MacroNewt.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class DeleteUserModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MacroNewtUser user)
        {
            return View(user);
        }
    }
}
