using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class ContactUsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ContactUsViewModel cuvm)
        {
            return View(cuvm);
        }
    }
}
