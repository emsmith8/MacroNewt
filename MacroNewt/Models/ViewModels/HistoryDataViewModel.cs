using MacroNewt.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Models.ViewModels
{
    public class HistoryDataViewModel
    {
        public List<Meal> Meals { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<DailyCalTotal> DailyTotals { get; set; }

        public HistoryDataViewModel()
        {
            Meals = new List<Meal>();
            DailyTotals = new List<DailyCalTotal>();
        }
    }
}
