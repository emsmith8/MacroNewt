using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class ResendConfirmationEmailViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
