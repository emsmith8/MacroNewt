using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class MealReviewViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(FoodViewModel vm)
        {
            return View(vm);
        }
    }
}
