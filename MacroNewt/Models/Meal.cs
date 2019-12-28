using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Models
{
    /*
     *  The Meal class
     *  Has public members for all relevant meal information
     */
    
    /// <summary>
    /// A Meal object includes all information relevant to a user created meal, including a list of <see cref="Food"/> items in the meal
    /// </summary>
    /// <remarks>
    /// Other relevant information includes the name and date of the meal, macronutrient totals, the ID and email of the user who logged the meal,
    ///     and an indicator of whether the meal is being relogged for avoiding unnecessary API calls for information already in database.
    /// </remarks>
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
