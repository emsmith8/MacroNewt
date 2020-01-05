using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        /// <summary>
        /// Main identifier for a Meal in the database.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name given to the meal by the user.
        /// </summary>
        [Display(Name = "Meal Title")]
        public string Title { get; set; }
        /// <summary>
        /// The type of meal as selected by user (i.e. 'snack', 'dinner').
        /// </summary>
        public string MealType { get; set; }
        /// <summary>
        /// The date and time at which the meal was logged. Represents the time the meal was eaten.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime MealDate { get; set; }
        /// <summary>
        /// A list of all <see cref="Food"/> items contained in the Meal.
        /// </summary>
        public List<Food> FoodComponents { get; set; }
        /// <summary>
        /// The total calorie value of the meal based on all component Foods.
        /// </summary>
        public int Calories { get; set; }
        /// <summary>
        /// The gram total protein content of the meal.
        /// </summary>
        public int ProteinCalories { get; set; }
        /// <summary>
        /// The gram total fat content of the meal.
        /// </summary>
        public int FatCalories { get; set; }
        /// <summary>
        /// The gram total carb content of the meal.
        /// </summary>
        public int CarbCalories { get; set; }
        /// <summary>
        /// The database ID of the user who logged the meal.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// The associated email adddress of the user who logged the meal.
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// An indicator of whether the meal is being re-logged. Provides user with previous portion/serving values if so.
        /// </summary>
        public bool ReLogged { get; set; }

    }
}
