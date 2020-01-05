using System.ComponentModel.DataAnnotations;

namespace MacroNewt.Models.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Required(ErrorMessage = "Please select a gender")]
        [DataType(DataType.Text)]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Height")]
        [Range(1, 7, ErrorMessage = "Please select height in feet")]
        public int HeightFeet { get; set; }
        [Range(0, 12, ErrorMessage = "Please select height in inches")]
        public int HeightInches { get; set; }

        [Range(1, 1000, ErrorMessage = "Weight must be 1-1000 lbs")]
        [Display(Name = "Weight (lbs)")]
        public int Weight { get; set; }

    }
}
