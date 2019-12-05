using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Models.ViewModels
{
    public class ConfirmMealViewModel
    {
        public bool ShowMacros { get; set; }
        public bool EditingMeal { get; set; }

        public int TargetCalories { get; set; }
        public int CurrentDayCalories { get; set; }
        public double CurrentDayCaloriesPercent { get; set; }
        public double PercentageCaloriesConsumed { get; set; }
        public int MealCalories { get; set; }
        public double MealCaloriesPercent { get; set; }
        public double MealCaloriesRemainingPercent { get; set; }
        
        public double TotalCaloriesSurplusPercent { get; set; }
        public double TotalCaloriesSurplusDisplayPercent { get; set; }
        public double TotalOldCaloriesSurplusDisplayPercent { get; set; }
        public double TotalNewCaloriesSurplusDisplayPercent { get; set; }

        public int MealCaloriesSurplus { get; set; }
        public double MealCaloriesSurplusUnderPercent { get; set; }
        public double MealCaloriesSurplusOverPercent { get; set; }
        public double MealCaloriesSurplusOverDisplayPercent { get; set; }

        public int TargetProteinCalories { get; set; }
        public int CurrentDayProteinCalories { get; set; }
        public double CurrentDayProteinCaloriesPercent { get; set; }
        public double PercentageProteinCaloriesConsumed { get; set; }
        public int MealProteinCalories { get; set; }
        public double MealProteinCaloriesPercent { get; set; }
        public double MealProteinCaloriesRemainingPercent { get; set; }

        public double TotalProteinCaloriesSurplusPercent { get; set; }
        public double TotalProteinCaloriesSurplusDisplayPercent { get; set; }
        public double TotalOldProteinCaloriesSurplusDisplayPercent { get; set; }
        public double TotalNewProteinCaloriesSurplusDisplayPercent { get; set; }

        public int MealProteinCaloriesSurplus { get; set; }
        public double MealProteinCaloriesSurplusUnderPercent { get; set; }
        public double MealProteinCaloriesSurplusOverPercent { get; set; }
        public double MealProteinCaloriesSurplusOverDisplayPercent { get; set; }

        public int TargetFatCalories { get; set; }
        public int CurrentDayFatCalories { get; set; }
        public double CurrentDayFatCaloriesPercent { get; set; }
        public double PercentageFatCaloriesConsumed { get; set; }
        public int MealFatCalories { get; set; }
        public double MealFatCaloriesPercent { get; set; }
        public double MealFatCaloriesRemainingPercent { get; set; }

        public double TotalFatCaloriesSurplusPercent { get; set; }
        public double TotalFatCaloriesSurplusDisplayPercent { get; set; }
        public double TotalOldFatCaloriesSurplusDisplayPercent { get; set; }
        public double TotalNewFatCaloriesSurplusDisplayPercent { get; set; }

        public int MealFatCaloriesSurplus { get; set; }
        public double MealFatCaloriesSurplusUnderPercent { get; set; }
        public double MealFatCaloriesSurplusOverPercent { get; set; }
        public double MealFatCaloriesSurplusOverDisplayPercent { get; set; }

        public int TargetCarbCalories { get; set; }
        public int CurrentDayCarbCalories { get; set; }
        public double CurrentDayCarbCaloriesPercent { get; set; }
        public double PercentageCarbCaloriesConsumed { get; set; }
        public int MealCarbCalories { get; set; }
        public double MealCarbCaloriesPercent { get; set; }
        public double MealCarbCaloriesRemainingPercent { get; set; }

        public double TotalCarbCaloriesSurplusPercent { get; set; }
        public double TotalCarbCaloriesSurplusDisplayPercent { get; set; }
        public double TotalOldCarbCaloriesSurplusDisplayPercent { get; set; }
        public double TotalNewCarbCaloriesSurplusDisplayPercent { get; set; }

        public int MealCarbCaloriesSurplus { get; set; }
        public double MealCarbCaloriesSurplusUnderPercent { get; set; }
        public double MealCarbCaloriesSurplusOverPercent { get; set; }
        public double MealCarbCaloriesSurplusOverDisplayPercent { get; set; }

        public string UserEmail { get; set; }
        public Boolean Admin { get; set; }
    }
}