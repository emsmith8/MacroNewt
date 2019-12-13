using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MacroNewt.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<MacroNewtUser> _userManager;
        private readonly SignInManager<MacroNewtUser> _signInManager;
        private readonly MacroNewtContext _context;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public DeletePersonalDataModel(
            UserManager<MacroNewtUser> userManager,
            SignInManager<MacroNewtUser> signInManager,
            MacroNewtContext context,
            ILogger<DeletePersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Password not correct.");
                    return Page();
                }
            }

            var result = await _userManager.DeleteAsync(user);

            var userMeals = _context.Meal
                .Where(m => m.UserId == user.Id);

            foreach (Meal m in userMeals)
            {
                _context.Meal.Remove(m);
            }

            await _context.SaveChangesAsync();

            var userStats = _context.DailyCalTotal
                .Where(s => s.Id == user.Id);

            foreach (DailyCalTotal stat in userStats)
            {
                _context.DailyCalTotal.Remove(stat);
            }

            await _context.SaveChangesAsync();

            var userGoals = _context.UserGoals
                .Where(g => g.Id == user.Id);

            foreach (UserGoals goal in userGoals)
            {
                _context.UserGoals.Remove(goal);
            }

            await _context.SaveChangesAsync();

            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleteing user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}