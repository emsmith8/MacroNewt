﻿using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MacroNewt.ViewComponents
{
    public class CurrentDayCalStatsViewComponent : ViewComponent
    {
        private MacroNewtContext _context { get; set; }

        public CurrentDayCalStatsViewComponent(MacroNewtContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(CurrentDayCalStatsViewModel cdcs)
        {
            return View(cdcs);
        }
    }
}
