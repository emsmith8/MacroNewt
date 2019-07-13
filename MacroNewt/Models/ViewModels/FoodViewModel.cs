using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Models.ViewModels
{
    public class FoodViewModel
    {        
        [Display(Name = "Total Calories"), Required]
        public int CalorieTotal { get; set; }

        public int ProteinTotal { get; set; }
        public int FatTotal { get; set; }
        public int CarbTotal { get; set; }
        
        [Display(Name = "Meal Name")]
        [Required(ErrorMessage = "Meal must have a name")]
        public string Title { get; set; }
        
        [Display(Name = "Meal Date")]
        [Required(ErrorMessage = "Meal must have a date")]
        public DateTime MealDate { get; set; }
        [Display(Name = "Meal Type: ")]
        [Required(ErrorMessage = "Meal must have a type")]
        public string MealType { get; set; }

        public List<Food> Foods {get;set;}

        [Display(Name = "Meals This Day")]
        public int MealCount { get; set; }

        public int MealId { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }

        public string ReLogged { get; set; }
        public string Edited { get; set; }

        public string SelectedPortionQty { get; set; }

        public FoodViewModel()
        {
            Foods = new List<Food>();
            CalorieTotal = 0;
            UserEmail = null;
            ReLogged = "false";
        }
    }
}
