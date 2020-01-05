using MacroNewt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Areas.Identity.Pages.Account.Manage
{
    public class UserGoalsModel : PageModel
    {
        private readonly UserManager<MacroNewtUser> _userManager;
        private readonly SignInManager<MacroNewtUser> _signInManager;
        private readonly MacroNewtContext _context;

        public UserGoalsModel(
            UserManager<MacroNewtUser> userManager,
            SignInManager<MacroNewtUser> signInManager,
            MacroNewtContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Overall goal")]
            public string Goal { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Pace")]
            public string Pace { get; set; }

            [Display(Name = "Calories from carbs %")]
            public int CarbPercent { get; set; }

            [Display(Name = "Calories from fat %")]
            public int FatPercent { get; set; }

            [Display(Name = "Calories from protein %")]
            public int ProteinPercent { get; set; }

            [HundredPercent(ErrorMessage = "The three macro percentages must add up to 100")]
            [Display(Name = "%")]
            public int PercentTotal { get; set; }

            [Display(Name = "Base daily calorie target")]
            public int BaseCalorieTarget { get; set; }

            public int CalAdjustment { get; set; }

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userGoal = await _context.UserGoals
                .Where(u => u.Id == user.Id)
                .FirstOrDefaultAsync();

            if (userGoal != null)
            {
                Input = new InputModel
                {
                    BaseCalorieTarget = userGoal.BaseCalorieTarget,
                    Goal = userGoal.OverallGoal,
                    Pace = userGoal.Pace,
                    CarbPercent = userGoal.CarbPercent,
                    FatPercent = userGoal.FatPercent,
                    ProteinPercent = userGoal.ProteinPercent,
                    PercentTotal = userGoal.CarbPercent + userGoal.FatPercent + userGoal.ProteinPercent,
                    CalAdjustment = userGoal.CalAdjustment
                };
            }
            else
            {
                Input = new InputModel
                {
                    BaseCalorieTarget = user.DailyTargetCalories
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var adjustedCals = Input.BaseCalorieTarget + Input.CalAdjustment;

            user.DailyTargetCalories = adjustedCals;

            _context.Users.Update(user);

            var userGoal = await _context.UserGoals
                .Where(u => u.Id == user.Id)
                .FirstOrDefaultAsync();

            if (userGoal == null)
            {
                userGoal = new UserGoals
                {
                    Id = user.Id,
                    BaseCalorieTarget = Input.BaseCalorieTarget,
                    CalAdjustment = Input.CalAdjustment,
                    OverallGoal = Input.Goal,
                    Pace = Input.Pace,
                    CarbPercent = Input.CarbPercent,
                    FatPercent = Input.FatPercent,
                    ProteinPercent = Input.ProteinPercent
                };

                _context.UserGoals.Add(userGoal);

                await _context.SaveChangesAsync();

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your goals have been updated";
                return RedirectToPage();
            }

            if (Input.BaseCalorieTarget != userGoal.BaseCalorieTarget)
            {
                userGoal.BaseCalorieTarget = Input.BaseCalorieTarget;
            }

            if (Input.CalAdjustment != userGoal.CalAdjustment)
            {
                userGoal.CalAdjustment = Input.CalAdjustment;
            }

            if (Input.Goal != userGoal.OverallGoal)
            {
                userGoal.OverallGoal = Input.Goal;
            }

            if (Input.Pace != userGoal.Pace)
            {
                userGoal.Pace = Input.Pace;
            }

            if (Input.CarbPercent != userGoal.CarbPercent)
            {
                userGoal.CarbPercent = Input.CarbPercent;
            }

            if (Input.FatPercent != userGoal.FatPercent)
            {
                userGoal.FatPercent = Input.FatPercent;
            }

            if (Input.ProteinPercent != userGoal.ProteinPercent)
            {
                userGoal.ProteinPercent = Input.ProteinPercent;
            }

            _context.UserGoals.Update(userGoal);

            await _context.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your goals have been updated";
            return RedirectToPage();
        }
    }


    public class HundredPercentAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;


            if ((int)value != 100)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

    }
}