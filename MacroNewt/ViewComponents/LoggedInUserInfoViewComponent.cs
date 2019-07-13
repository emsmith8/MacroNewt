using MacroNewt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class LoggedInUserInfoViewComponent : ViewComponent
    {
        private readonly UserManager<MacroNewtUser> _userManager;
        private readonly MacroNewtContext _context;

        public LoggedInUserInfoViewComponent(UserManager<MacroNewtUser> userManager, MacroNewtContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                var nonExistingDayCal = new DailyCalTotal();
                return View(nonExistingDayCal);
            }
            else
            {
                var existingDayCal = await _context.DailyCalTotal
                .Where(x => (x.CalorieDay == DateTime.Today) && (x.Id == user.Id))
                .FirstOrDefaultAsync();

                if (User.IsInRole("Admin"))
                {
                    return View(new DailyCalTotal());
                }

                return View(existingDayCal);
            }
        }
    }
}
