using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class ConfirmContact : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
