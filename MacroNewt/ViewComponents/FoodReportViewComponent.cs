using MacroNewt.Models;
using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class FoodReportViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Food food)
        {
            return View(food);
        }
    }
}