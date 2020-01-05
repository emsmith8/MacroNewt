using MacroNewt.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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
