using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models.LogicModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MacroNewt.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<MacroNewtUser> _userManager;
        private readonly SignInManager<MacroNewtUser> _signInManager;
        private readonly MacroNewtContext _context;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<MacroNewtUser> userManager,
            SignInManager<MacroNewtUser> signInManager,
            MacroNewtContext context,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
        }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

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
            public string ProfileName { get; set; }
            
            [DataType(DataType.Text)]
            [Display(Name = "Gender")]
            public string Gender { get; set; }

            [Display(Name = "Height")]
            public int HeightFeet { get; set; }
            public int HeightInches { get; set; }

            [Display(Name = "Weight (lbs)")]
            public int Weight { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            
            Input = new InputModel
            {
                Name = user.Name,
                DOB = user.DOB,
                ProfileName = user.ProfileName,
                Gender = user.Gender,
                HeightFeet = user.HeightFeet,
                HeightInches = user.HeightInches,
                Weight = user.Weight,
                Email = email,
                PhoneNumber = phoneNumber
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

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

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            if (Input.ProfileName != user.ProfileName)
            {
                user.ProfileName = Input.ProfileName;
            }
            
            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            if (Input.DOB != user.DOB)
            {
                user.DOB = Input.DOB;
            }
            
            if (Input.Gender != user.Gender)
            {
                user.Gender = Input.Gender;
            }

            if (Input.HeightFeet != user.HeightFeet)
            {
                user.HeightFeet = Input.HeightFeet;
            }

            if (Input.HeightInches != user.HeightInches)
            {
                user.HeightInches = Input.HeightInches;
            }

            if (Input.Weight != user.Weight)
            {
                user.Weight = Input.Weight;
            }

            var age = DateTime.Today.Year - Input.DOB.Year;

            if (Input.DOB.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            user.Age = age;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
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

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            EmailBuildHandler ebh = new EmailBuildHandler();

            string finalEmail = ebh.BuildVerificationEmailHtml(user.Name, user.Email, callbackUrl);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", finalEmail);

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
