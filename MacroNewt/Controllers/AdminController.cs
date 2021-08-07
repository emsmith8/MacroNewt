using MacroNewt.Application.Common.Interfaces;
using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MacroNewt.Controllers
{
    /// <summary>
    /// Controller that displays views that support application administration tasks.
    /// </summary>
    public class AdminController : Controller
    {
        private readonly IMacroNewtDbContext _context;
        private readonly UserManager<MacroNewtUser> _userManager;

        public AdminController(IMacroNewtDbContext context, UserManager<MacroNewtUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdministrator")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> ManageMeals()
        {
            ViewBag.Title = "Manage Meals";
            return View(await _context.Meals.ToListAsync());
        }

        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> ManageUsers()
        {
            ViewBag.Title = "Manage Users";
            return View(await _context.Users.ToListAsync());
        }

        // GET
        public IActionResult EditUserDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();

            return View(user);
            
        }

        // GET
        public IActionResult ReviewUserDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserDetails([Bind("ProfileName,Name,DOB,Gender,Weight,DailyTargetCalories,Id")] MacroNewtUser user)
        {
            if (ModelState.IsValid)
            {
                var userToUpdate = _context.Users
                    .Where(u => u.Id == user.Id)
                    .FirstOrDefault();

                if (userToUpdate != null)
                {
                    try
                    {
                        userToUpdate.ProfileName = user.ProfileName;
                        userToUpdate.Name = user.Name;
                        userToUpdate.DOB = user.DOB;
                        userToUpdate.Gender = user.Gender;
                        userToUpdate.Weight = user.Weight;
                        userToUpdate.DailyTargetCalories = user.DailyTargetCalories;
                        
                        _context.Users.Update(userToUpdate);

                        await _context.SaveChangesAsync(new CancellationToken());
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("ManageUsers", "Admin");
                }
                
            }

            return View(user);
        }
        
        // GET
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return ViewComponent("DeleteUserModal", user);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(new CancellationToken());

            var userMeals = _context.Meals
                .Where(m => m.UserId == id);

            foreach (Meal m in userMeals)
            {
                _context.Meals.Remove(m);
            }

            await _context.SaveChangesAsync(new CancellationToken());


            var userStats = _context.DailyCalTotals
                .Where(s => s.Id == id);

            foreach (DailyCalTotal stat in userStats)
            {
                _context.DailyCalTotals.Remove(stat);
            }
            
            await _context.SaveChangesAsync(new CancellationToken());

            return RedirectToAction("ManageUsers", "Admin");
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
    }
}