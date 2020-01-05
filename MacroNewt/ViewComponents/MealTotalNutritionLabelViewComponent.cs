using MacroNewt.Models;
using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class MealTotalNutritionLabelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MealTotalNutrient mtn)
        {
            return View(mtn);
        }
    }
}
