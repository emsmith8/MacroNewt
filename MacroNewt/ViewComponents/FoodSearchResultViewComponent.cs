using MacroNewt.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MacroNewt.ViewComponents
{
    public class FoodSearchResultViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<Food> foods)
        {
            return View(foods);
        }
    }
}
