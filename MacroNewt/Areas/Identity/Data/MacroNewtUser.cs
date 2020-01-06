using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MacroNewt.Areas.Identity.Data
{
    /*
     *  The MacroNewtUser class
     *  Extends the IdentityUser class and has public members for all relevant user information
     */

    /// <summary>
    /// A MacroNewtUser object includes all information relevant to a user account, including the user's <see cref="UserGoals"/> and a list of
    ///     all <see cref="DailyCalTotal"/> records associated with the user.
    /// </summary>
    public class MacroNewtUser : IdentityUser
    {
        /// <summary>
        /// The name assigned to an account by the user.
        /// </summary>
        [PersonalData]
        public string Name { get; set; }
        /// <summary>
        /// The user's date of birth.
        /// </summary>
        [PersonalData]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DOB { get; set; }
        /// <summary>
        /// Currently the same as the Name property.
        /// </summary>
        [PersonalData]
        public string ProfileName { get; set; }
        /// <summary>
        /// The age of the user, relevant for setting dietary goals.
        /// </summary>
        [PersonalData]
        public int Age { get; set; }
        /// <summary>
        /// The gender of the user, relevant for setting dietary goals but not required.
        /// </summary>
        [PersonalData]
        public string Gender { get; set; }
        /// <summary>
        /// The foot height of the user, relevant for setting dietary goals but not required.
        /// </summary>
        [PersonalData]
        public int HeightFeet { get; set; }
        /// <summary>
        /// The inch height of the user.
        /// </summary>
        [PersonalData]
        public int HeightInches { get; set; }
        /// <summary>
        /// The weight of the user, relevant for setting dietary goals but not required.
        /// </summary>
        [PersonalData]
        public int Weight { get; set; }
        /// <summary>
        /// The currently chosen target daily calories for the user.
        /// </summary>
        [PersonalData]
        public int DailyTargetCalories { get; set; }
        /// <summary>
        /// Virtual property to allow for EF lazy loading of associated <see cref="UserGoals"/> record.
        /// </summary>
        public virtual UserGoals Goals { get; set; }
        /// <summary>
        /// Virtual property to allow for EF lazy loading of associated <see cref="DailyCalTotal"/> record.
        /// </summary>
        public virtual List<DailyCalTotal> DailyCalTotals { get; set; }

    }

    /*
     *  The UserGoals class
     *  Has public members for all relevant user goal information
     */

    /// <summary>
    /// A UserGoals object includes all information relevant to the user's currently targeted goals.
    /// </summary>
    public class UserGoals
    {
        /// <summary>
        /// The base daily calorie target chosen by a user.
        /// </summary>
        public int BaseCalorieTarget { get; set; }
        /// <summary>
        /// The adjustment to be made to the base calorie target according to the user's selected goals (e.g. lose fat at relaxed pace).
        /// </summary>
        public int CalAdjustment { get; set; }
        /// <summary>
        /// The dietary goal selected by the user (e.g. lose fat). Not required to have an account.
        /// </summary>
        public string OverallGoal { get; set; }
        /// <summary>
        /// The user's chosen pace of approaching selected goal.
        /// </summary>
        public string Pace { get; set; }
        /// <summary>
        /// Target percent of daily calories to come from carbohydrates as chosen by user.
        /// </summary>
        public int CarbPercent { get; set; }
        /// <summary>
        /// Target percent of daily calories to come from fat as chosen by user.
        /// </summary>
        public int FatPercent { get; set; }
        /// <summary>
        /// Target percent of daily calories to come from protein as chosen by user.
        /// </summary>
        public int ProteinPercent { get; set; }
        /// <summary>
        /// Virtual property to allow for EF lazy loading of associated <see cref="MacroNewtUser"/> record.
        /// </summary>
        public virtual MacroNewtUser MacroNewtUser { get; set; }
        /// <summary>
        /// The string identifier for the record of a user's goals, included to allow EF to match up user and goal record.
        /// </summary>
        public string Id { get; set; }
    }

    /*
     *  The DailyCalTotal class
     *  Has public members for all relevant daily calorie stats
     */

    /// <summary>
    /// A DailyCalTotal object includes all information relevant to the user's daily consumption of both overall and macronutrient based calories.
    /// </summary>
    public class DailyCalTotal
    {
        /// <summary>
        /// The ID of a user's daily stats record.
        /// </summary>
        [Key]
        public int DailyCalTotalId { get; set; }
        /// <summary>
        /// The total calories consumed by the user on the given day.
        /// </summary>
        public int TotalDailyCalories { get; set; }
        /// <summary>
        /// The total daily calories currently targeted by the user.
        /// </summary>
        public int TargetDailyCalories { get; set; }
        /// <summary>
        /// The total calories from protein sources consumed by the user on the given day.
        /// </summary>
        public int TotalDailyProteinCalories { get; set; }
        /// <summary>
        /// The total calories from fat sources consumed by the user on the given day.
        /// </summary>
        public int TotalDailyFatCalories { get; set; }
        /// <summary>
        /// The total calories from carb sources consumed by the user on the given day.
        /// </summary>
        public int TotalDailyCarbCalories { get; set; }
        /// <summary>
        /// The date for which the record represents daily calorie stats.
        /// </summary>
        public DateTime CalorieDay { get; set; }
        /// <summary>
        /// Virtual property to allow for EF lazy loading of associated <see cref="MacroNewtUser"/> record.
        /// </summary>
        public virtual MacroNewtUser MacroNewtUser { get; set; }
        /// <summary>
        /// The string identifier for the record of a user's daily stats, included to allow EF to match up user and stats record.
        /// </summary>
        public string Id { get; set; }

    }
}
