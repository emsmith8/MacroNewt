using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class MealDetailViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(FoodViewModel vm)
        {
            return View(vm);
        }
    }
}
