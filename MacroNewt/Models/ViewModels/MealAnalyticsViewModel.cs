using MacroNewt.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Models.ViewModels
{
    public class MealAnalyticsViewModel
    {
        public List<DailyCalTotal> DailyTotals { get; set; }

        public int FatMonthTotal { get; set; }
        public int CarbMonthTotal { get; set; }
        public int ProteinMonthTotal { get; set; }

    }
}
