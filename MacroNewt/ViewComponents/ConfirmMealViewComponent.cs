using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
