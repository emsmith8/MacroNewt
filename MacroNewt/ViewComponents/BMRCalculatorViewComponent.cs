using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class BMRCalculatorViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(BMRCalculatorViewModel bmrc)
        {
            return View(bmrc);
        }
    }
}
