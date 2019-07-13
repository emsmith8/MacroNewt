using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Models.ViewModels
{
    public class CurrentDayCalStatsViewModel
    {
        public bool ShowMacros { get; set; }

        public int TargetCalories { get; set; }
        public int CurrentDayCalories { get; set; }
        public double CurrentDayCaloriesPercent { get; set; }
        public double CaloriesRemainingPercent { get; set; }
        
        public double TotalCaloriesSurplusPercent { get; set; }
        public double TotalCaloriesSurplusDisplayPercent { get; set; }

        public int TargetProteinCalories { get; set; }
        public int CurrentDayProteinCalories { get; set; }
        public double CurrentDayProteinCaloriesPercent { get; set; }
        public double CurrentDayProteinCaloriesRemainingPercent { get; set; }

        public double TotalProteinCaloriesSurplusPercent { get; set; }
        public double TotalProteinCaloriesSurplusDisplayPercent { get; set; }

        public int TargetFatCalories { get; set; }
        public int CurrentDayFatCalories { get; set; }
        public double CurrentDayFatCaloriesPercent { get; set; }
        public double CurrentDayFatCaloriesRemainingPercent { get; set; }

        public double TotalFatCaloriesSurplusPercent { get; set; }
        public double TotalFatCaloriesSurplusDisplayPercent { get; set; }

        public int TargetCarbCalories { get; set; }
        public int CurrentDayCarbCalories { get; set; }
        public double CurrentDayCarbCaloriesPercent { get; set; }
        public double CurrentDayCarbCaloriesRemainingPercent { get; set; }

        public double TotalCarbCaloriesSurplusPercent { get; set; }
        public double TotalCarbCaloriesSurplusDisplayPercent { get; set; }
    }
}
