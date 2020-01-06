using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Controllers
{
    /* 
     *  The Admin controller
     *  Handles administrative interactions with database for managing meals and users and returns appropriate views
    */

    /// <summary>
    /// Controller that displays views that support application administration tasks.
    /// </summary>
    /// <seealso cref="Controller"/>
    public class AdminController : Controller
    {
        private readonly MacroNewtContext _context;
        private readonly UserManager<MacroNewtUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="context">The dependency-injected <see cref="MacroNewtContext"/> obtained from the <see cref="Startup.ConfigureServices"/></param>
        /// <param name="userManager">The dependency-injected <see cref="UserManager{TUser}"/></param>
        public AdminController(MacroNewtContext context, UserManager<MacroNewtUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns the admin homepage view
        /// </summary>
        /// <returns>The Views/Admin/Index <see cref="ViewResult"/></returns>
        [Authorize(Policy = "RequireAdministrator")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns a view containing a list of all logged user meals
        /// </summary>
        /// <returns>The Views/Admin/ManageMeals <see cref="ViewResult"/></returns>
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> ManageMeals()
        {
            ViewBag.Title = "Manage Meals";
            return View(await _context.Meal.ToListAsync());
        }

        /// <summary>
        /// Returns a view containing a list of all <see cref="MacroNewtUser"/> accounts
        /// </summary>
        /// <returns>The Admin/ManageUsers <see cref="ViewResult"/></returns>
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> ManageUsers()
        {
            ViewBag.Title = "Manage Users";

            var userList = await _context.Users.ToListAsync();

            var adminUser = userList.Single(u => u.Name == "Admin");

            userList.Remove(adminUser);

            return View(userList);
        }

        /// <summary>
        /// Returns a view with specified <see cref="MacroNewtUser"/> details ready for editing
        /// </summary>
        /// <param name="id">The string identifier of the user account to be edited</param>
        /// <returns>The Views/Admin/EditUserDetails <see cref="ViewResult"/></returns>
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

        /// <summary>
        /// Returns a view with specified <see cref="MacroNewtUser"/> details for review
        /// </summary>
        /// <param name="id">The string identifier of the user account to be reviewed</param>
        /// <returns>The Views/Admin/ReviewUserDetails <see cref="ViewResult"/></returns>
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

        /// <summary>
        /// Edits target <see cref="MacroNewtUser"/> account details as provided by admin
        /// </summary>
        /// <param name="user">The target user account to be edited</param>
        /// <returns>A redirect to previous location if post is valid</returns>
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

                        await _context.SaveChangesAsync();
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

        /// <summary>
        /// Gets the user delete view component
        /// </summary>
        /// <param name="id">The string identifier of the user account to be deleted</param>
        /// <returns>The DeleteUserModal ViewComponent</returns>
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

        /// <summary>
        /// Deletes target <see cref="MacroNewtUser"/> with specified id
        /// </summary>
        /// <param name="id">The string identifier of the user account to be deleted</param>
        /// <returns>A redirect to previous location</returns>
        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            var userMeals = _context.Meal
                .Where(m => m.UserId == id);

            foreach (Meal m in userMeals)
            {
                _context.Meal.Remove(m);
            }

            await _context.SaveChangesAsync();


            var userStats = _context.DailyCalTotal
                .Where(s => s.Id == id);

            foreach (DailyCalTotal stat in userStats)
            {
                _context.DailyCalTotal.Remove(stat);
            }

            await _context.SaveChangesAsync();

            var userGoals = _context.UserGoals
                .Where(g => g.Id == id);

            foreach (UserGoals goal in userGoals)
            {
                _context.UserGoals.Remove(goal);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ManageUsers", "Admin");
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
    }
}