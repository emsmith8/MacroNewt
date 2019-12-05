using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Areas.Identity.Data
{
    public class MacroNewtUser : IdentityUser
    {
 //       public Guid UserId { get; set; }

        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DOB { get; set; }

        [PersonalData]
        public string ProfileName { get; set; }

        [PersonalData]
        public int Age { get; set; }

        [PersonalData]
        public string Gender { get; set; }

        [PersonalData]
        public int HeightFeet { get; set; }
        [PersonalData]
        public int HeightInches { get; set; }

        [PersonalData]
        public int Weight { get; set; }

        [PersonalData]
        public int DailyTargetCalories { get; set; }

        public virtual UserGoals Goals { get; set; }
        public virtual List<DailyCalTotal> DailyCalTotals { get; set; }
        
    }

    public class UserGoals
    {
        public int BaseCalorieTarget { get; set; }
        public int CalAdjustment { get; set; }

        public string OverallGoal { get; set; }
        public string Pace { get; set; }

        public int CarbPercent { get; set; }
        public int FatPercent { get; set; }
        public int ProteinPercent { get; set; }

        public virtual MacroNewtUser MacroNewtUser { get; set; }
        public string Id { get; set; }
    }

    public class DailyCalTotal
    {
        [Key]
        public int DailyCalTotalId { get; set; }
        public int TotalDailyCalories { get; set; }

        public int TargetDailyCalories { get; set; }

        public int TotalDailyProteinCalories { get; set; }
        public int TotalDailyFatCalories { get; set; }
        public int TotalDailyCarbCalories { get; set; }

        public DateTime CalorieDay { get; set; }

        public virtual MacroNewtUser MacroNewtUser { get; set; }
        public string Id { get; set; }
        
    }
}
