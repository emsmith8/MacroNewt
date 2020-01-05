using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class MealAnalyticsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MealAnalyticsViewModel mav)
        {
            return View(mav);
        }
    }
}
