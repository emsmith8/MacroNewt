using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models.LogicModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MacroNewt.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<MacroNewtUser> _signInManager;
        private readonly UserManager<MacroNewtUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<MacroNewtUser> userManager,
            SignInManager<MacroNewtUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Birth Date")]
            [DataType(DataType.Date)]
            public DateTime DOB { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public string ProfileName { get; set; }
            
            //[DataType(DataType.Text)]
            //[Display(Name = "Gender")]
            //public string Gender { get; set; }
            
            //[Display(Name = "Height")]
            //public int HeightFeet { get; set; }
            //public int HeightInches { get; set; }

            //[Display(Name = "Weight (lbs)")]
            //public int Weight { get; set; }

            //[Display(Name = "Daily Calorie Target")]
            //public int DailyCalorieTarget { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [EmailAddress]
            [Display(Name = "Confirm Email")]
            public string ConfirmEmail { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var age = DateTime.Today.Year - Input.DOB.Year;

                if (Input.DOB.Date > DateTime.Today.AddYears(-age))
                {
                    age--;
                }

                var user = new MacroNewtUser {
                    Name = Input.Name,
                    DOB = Input.DOB,
                    //Gender = Input.Gender,
                    Age = age,
                    HeightFeet = -1,
                    HeightInches = -1,
                    Weight = 0,
                    DailyTargetCalories = 2000,
                    UserName = Input.Email,
                    ProfileName = Input.ProfileName,
                    Email = Input.Email 
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    EmailBuildHandler ebh = new EmailBuildHandler();

                    string finalEmail = ebh.BuildVerificationEmailHtml(user.Name, user.Email, callbackUrl);

                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email", finalEmail);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    if (error.Description.StartsWith("User name"))
                    {
                        ModelState.AddModelError(string.Empty, "Email address '" + Input.Email + "' is already taken");
                        error.Description.Replace("User name", "Email address");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
