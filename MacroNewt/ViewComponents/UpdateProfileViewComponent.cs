using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
