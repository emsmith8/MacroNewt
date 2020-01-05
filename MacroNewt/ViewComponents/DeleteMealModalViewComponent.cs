using MacroNewt.Models;
using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class DeleteMealModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Meal meal)
        {
            return View(meal);
        }
    }
}
