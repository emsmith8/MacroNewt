using MacroNewt.Models;
using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class NoMeasureFoodNutritionLabelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(NoMeasureFood nmf)
        {
            return View(nmf);
        }
    }
}
