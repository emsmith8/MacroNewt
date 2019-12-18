using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Models.ViewModels
{
    public class ContactUsViewModel
    {
        [Display(Name = "Your email address")]
        [EmailAddress]
        [Required(ErrorMessage = "Must provide your email")]
        public string UserEmail { get; set; }
        [Display(Name = "Reason for contact")]
        [Required(ErrorMessage = "Must provide reason for contact")]
        public string ContactType { get; set; }
        [Display(Name = "Your message")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Must provide a message")]
        public string Message { get; set; }
    }

}
