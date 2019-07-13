using MacroNewt.Models;
using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class DateRangeHistoryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(HistoryDataViewModel hd)
        {
            return View(hd);
        }
    }
}
