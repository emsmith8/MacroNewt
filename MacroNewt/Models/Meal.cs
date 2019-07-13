using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Models
{
    public class Meal
    {
        public int Id { get; set; }
        [Display(Name = "Meal Title")]
        public string Title { get; set; }
        public string MealType { get; set; }
        [DataType(DataType.Date)]
        public DateTime MealDate { get; set; }
        public List<Food> FoodComponents { get; set; }
        public int Calories { get; set; }

        public int ProteinCalories { get; set; }
        public int FatCalories { get; set; }
        public int CarbCalories { get; set; }

        public string UserId { get; set; }
        public string UserEmail { get; set; }

        public bool ReLogged { get; set; }

    }
}
