using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MacroNewt.Models.LogicModels
{
    public class UserStatsHandler
    {
        private readonly UserManager<MacroNewtUser> _userManager;
        private readonly MacroNewtContext _context;

        public UserStatsHandler(UserManager<MacroNewtUser> userManager, MacroNewtContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public CurrentDayCalStatsViewModel OrganizeCalStats(MacroNewtUser user, int targetTotalCal, DailyCalTotal dct)
        {
            if (dct == null)
            {
                dct = new DailyCalTotal();
            }

            double proteinBal = 0.2;
            double proteinTar = 0;
            double fatBal = 0.25;
            double fatTar = 0;
            double carbBal = 0.55;
            double carbTar = 0;

            var showMacros = _context.UserGoals
                .Where(g => g.Id == user.Id)
                .FirstOrDefault();

            if (showMacros != null)
            {
                proteinBal = ((double)showMacros.ProteinPercent / 100);
                fatBal = ((double)showMacros.FatPercent / 100);
                carbBal = ((double)showMacros.CarbPercent / 100);

                proteinTar = (double)targetTotalCal * proteinBal;
                fatTar = ((double)targetTotalCal * fatBal);
                carbTar = ((double)targetTotalCal * carbBal);
            }

            CurrentDayCalStatsViewModel cdcsvm = new CurrentDayCalStatsViewModel()
            {
                TargetCalories = targetTotalCal,
                CurrentDayCalories = dct.TotalDailyCalories,
                CurrentDayCaloriesPercent = Math.Round(((dct.TotalDailyCalories * 100) / (targetTotalCal * 1.5))),
                PercentageCaloriesConsumed = Math.Round(dct.TotalDailyCalories * 100.0 / targetTotalCal),
                TargetProteinCalories = Convert.ToInt32(proteinTar),
                //TargetProteinCalories = (targetTotalCal / 5),
                CurrentDayProteinCalories = dct.TotalDailyProteinCalories,
                CurrentDayProteinCaloriesPercent = Math.Round(((dct.TotalDailyProteinCalories * 100) / (proteinTar * 1.5))),
                PercentageProteinCaloriesConsumed = Math.Round(dct.TotalDailyProteinCalories * 100.0 / proteinTar),
                //CurrentDayProteinCaloriesPercent = Math.Round(((dct.TotalDailyProteinCalories * 100) / (targetTotalCal * 1.5 / 5))),
                TargetFatCalories = Convert.ToInt32(fatTar),
                //TargetFatCalories = (targetTotalCal / 3),
                CurrentDayFatCalories = dct.TotalDailyFatCalories,
                CurrentDayFatCaloriesPercent = Math.Round(((dct.TotalDailyFatCalories * 100) / (fatTar * 1.5))),
                PercentageFatCaloriesConsumed = Math.Round(dct.TotalDailyFatCalories * 100.0 / fatTar),
                //CurrentDayFatCaloriesPercent = Math.Round(((dct.TotalDailyFatCalories * 100) / (targetTotalCal * 1.5 / 3))),
                TargetCarbCalories = Convert.ToInt32(carbTar),
                //TargetCarbCalories = (targetTotalCal / 2),
                CurrentDayCarbCalories = dct.TotalDailyCarbCalories,
                CurrentDayCarbCaloriesPercent = Math.Round(((dct.TotalDailyCarbCalories * 100) / (carbTar * 1.5))),
                PercentageCarbCaloriesConsumed = Math.Round(dct.TotalDailyCarbCalories * 100.0 / carbTar)
                //CurrentDayCarbCaloriesPercent = Math.Round(((dct.TotalDailyCarbCalories * 100) / (targetTotalCal * 1.5 / 2)))
            };




            if (showMacros != null)
            {
                cdcsvm.ShowMacros = true;
            }

            if (dct.TotalDailyCalories > targetTotalCal)
            {
                cdcsvm.TotalCaloriesSurplusPercent = Math.Round(((dct.TotalDailyCalories) / (double)targetTotalCal) * 100);
                cdcsvm.TotalCaloriesSurplusDisplayPercent = Math.Round(((dct.TotalDailyCalories - targetTotalCal) * 100) / ((double)targetTotalCal * 1.5));
                if (cdcsvm.TotalCaloriesSurplusDisplayPercent > 38)
                {
                    cdcsvm.TotalCaloriesSurplusDisplayPercent = 38;
                }
            }
            else
            {
                cdcsvm.CaloriesRemainingPercent = (67 - (cdcsvm.CurrentDayCaloriesPercent));
            }

            if (cdcsvm.CurrentDayProteinCalories > cdcsvm.TargetProteinCalories)
            {
                cdcsvm.TotalProteinCaloriesSurplusPercent = Math.Round(((cdcsvm.CurrentDayProteinCalories) / (double)cdcsvm.TargetProteinCalories) * 100);
                cdcsvm.TotalProteinCaloriesSurplusDisplayPercent = Math.Round(((cdcsvm.CurrentDayProteinCalories - cdcsvm.TargetProteinCalories) * 100) / ((double)cdcsvm.TargetProteinCalories * 1.5));
                if (cdcsvm.TotalProteinCaloriesSurplusDisplayPercent > 38)
                {
                    cdcsvm.TotalProteinCaloriesSurplusDisplayPercent = 38;
                }
            }
            else
            {
                cdcsvm.CurrentDayProteinCaloriesRemainingPercent = (67 - (cdcsvm.CurrentDayProteinCaloriesPercent));
            }

            if (cdcsvm.CurrentDayFatCalories > cdcsvm.TargetFatCalories)
            {
                cdcsvm.TotalFatCaloriesSurplusPercent = Math.Round(((cdcsvm.CurrentDayFatCalories) / (double)cdcsvm.TargetFatCalories) * 100);
                cdcsvm.TotalFatCaloriesSurplusDisplayPercent = Math.Round(((cdcsvm.CurrentDayFatCalories - cdcsvm.TargetFatCalories) * 100) / ((double)cdcsvm.TargetFatCalories * 1.5));
                if (cdcsvm.TotalFatCaloriesSurplusDisplayPercent > 38)
                {
                    cdcsvm.TotalFatCaloriesSurplusDisplayPercent = 38;
                }
            }
            else
            {
                cdcsvm.CurrentDayFatCaloriesRemainingPercent = (67 - (cdcsvm.CurrentDayFatCaloriesPercent));
            }

            if (cdcsvm.CurrentDayCarbCalories > cdcsvm.TargetCarbCalories)
            {
                cdcsvm.TotalCarbCaloriesSurplusPercent = Math.Round(((cdcsvm.CurrentDayCarbCalories) / (double)cdcsvm.TargetCarbCalories) * 100);
                cdcsvm.TotalCarbCaloriesSurplusDisplayPercent = Math.Round(((cdcsvm.CurrentDayCarbCalories - cdcsvm.TargetCarbCalories) * 100) / ((double)cdcsvm.TargetCarbCalories * 1.5));
                if (cdcsvm.TotalCarbCaloriesSurplusDisplayPercent > 38)
                {
                    cdcsvm.TotalCarbCaloriesSurplusDisplayPercent = 38;
                }
            }
            else
            {
                cdcsvm.CurrentDayCarbCaloriesRemainingPercent = (67 - (cdcsvm.CurrentDayCarbCaloriesPercent));
            }

            return cdcsvm;
        }

        public CurrentDayCalStatsViewModel GetPastMacroTargets(MacroNewtUser user, string targetDate)
        {

            int targetTotalCal = _context.Users
                .Where(u => u.Id == user.Id)
                .Select(u => u.DailyTargetCalories)
                .FirstOrDefault();

            DailyCalTotal dct = _context.DailyCalTotal
                .Where(d => (d.Id == user.Id) && (d.CalorieDay.Date == Convert.ToDateTime(targetDate)))
                .FirstOrDefault();

            return OrganizeCalStats(user, targetTotalCal, dct);
        }

        public CurrentDayCalStatsViewModel GetCurrentMacroTargets(MacroNewtUser user)
        {
            int targetTotalCal = _context.Users
                .Where(u => u.Id == user.Id)
                .Select(u => u.DailyTargetCalories)
                .FirstOrDefault();

            DailyCalTotal dct = _context.DailyCalTotal
                .Where(d => (d.Id == user.Id) && (d.CalorieDay == DateTime.Today))
                .FirstOrDefault();

            return OrganizeCalStats(user, targetTotalCal, dct);

        }

        public ConfirmMealViewModel GetMacroTargets(MacroNewtUser user, Boolean admin, int mealId, DateTime date, int mealCalories, int mealProtein, int mealFat, int mealCarb)
        {
            string UserId = user.Id;

            int targetTotalCal = _context.Users
                    .Where(u => u.Id == UserId)
                    .Select(u => u.DailyTargetCalories)
                    .FirstOrDefault();

            DailyCalTotal dct = _context.DailyCalTotal
                .Where(d => (d.Id == UserId) && (d.CalorieDay.Date == date.Date))
                .FirstOrDefault();

            if (dct == null)
            {
                dct = new DailyCalTotal
                {
                    Id = UserId
                };
            }




            double proteinBal = 0.2;
            double proteinTar = 0;
            double fatBal = 0.25;
            double fatTar = 0;
            double carbBal = 0.55;
            double carbTar = 0;

            var showMacros = _context.UserGoals
                .Where(g => g.Id == user.Id)
                .FirstOrDefault();

            if (showMacros != null)
            {
                proteinBal = ((double)showMacros.ProteinPercent / 100);
                fatBal = ((double)showMacros.FatPercent / 100);
                carbBal = ((double)showMacros.CarbPercent / 100);

                proteinTar = (double)targetTotalCal * proteinBal;
                fatTar = ((double)targetTotalCal * fatBal);
                carbTar = ((double)targetTotalCal * carbBal);
            }




            if (mealId != 0)
            {
                var oldMeal = _context.Meal
                    .Where(m => m.Id == mealId)
                    .FirstOrDefault();

                dct.TotalDailyCalories = (dct.TotalDailyCalories - oldMeal.Calories);
                dct.TotalDailyProteinCalories = (dct.TotalDailyProteinCalories - oldMeal.ProteinCalories);
                dct.TotalDailyFatCalories = (dct.TotalDailyFatCalories - oldMeal.FatCalories);
                dct.TotalDailyCarbCalories = (dct.TotalDailyCarbCalories - oldMeal.CarbCalories);
            }

            ConfirmMealViewModel cmvm = new ConfirmMealViewModel()
            {
                TargetCalories = targetTotalCal,
                CurrentDayCalories = dct.TotalDailyCalories,
                CurrentDayCaloriesPercent = Math.Round(((dct.TotalDailyCalories * 100) / (targetTotalCal * 1.5))),
                PercentageCaloriesConsumed = Math.Round((dct.TotalDailyCalories + mealCalories) * 100.0 / targetTotalCal),
                MealCalories = mealCalories,
                MealCaloriesPercent = ((mealCalories * 100) / (targetTotalCal * 1.5)),
                TargetProteinCalories = Convert.ToInt32(proteinTar),
                //TargetProteinCalories = (targetTotalCal / 5),
                CurrentDayProteinCalories = dct.TotalDailyProteinCalories,
                CurrentDayProteinCaloriesPercent = Math.Round(((dct.TotalDailyProteinCalories * 100) / (proteinTar * 1.5))),
                PercentageProteinCaloriesConsumed = Math.Round((dct.TotalDailyProteinCalories + (mealProtein * 4)) * 100.0 / proteinTar),
                //CurrentDayProteinCaloriesPercent = Math.Round(((dct.TotalDailyProteinCalories * 100) / (targetTotalCal * 1.5 / 5))),
                MealProteinCalories = (mealProtein * 4),
                MealProteinCaloriesPercent = ((mealProtein * 4 * 100) / (proteinTar * 1.5)),
                //MealProteinCaloriesPercent = ((mealProtein * 4 * 100) / (targetTotalCal * 1.5 / 5)),
                TargetFatCalories = Convert.ToInt32(fatTar),
                //TargetFatCalories = (targetTotalCal / 3),
                CurrentDayFatCalories = dct.TotalDailyFatCalories,
                CurrentDayFatCaloriesPercent = Math.Round(((dct.TotalDailyFatCalories * 100) / (fatTar * 1.5))),
                PercentageFatCaloriesConsumed = Math.Round((dct.TotalDailyFatCalories + (mealFat * 9)) * 100.0 / fatTar),
                //CurrentDayFatCaloriesPercent = Math.Round(((dct.TotalDailyFatCalories * 100) / (targetTotalCal * 1.5 / 3))),
                MealFatCalories = (mealFat * 9),
                MealFatCaloriesPercent = ((mealFat * 9 * 100) / (fatTar * 1.5)),
                //MealFatCaloriesPercent = ((mealFat * 9 * 100) / (targetTotalCal * 1.5 / 3)),
                TargetCarbCalories = Convert.ToInt32(carbTar),
                //TargetCarbCalories = (targetTotalCal / 2),
                CurrentDayCarbCalories = dct.TotalDailyCarbCalories,
                CurrentDayCarbCaloriesPercent = Math.Round(((dct.TotalDailyCarbCalories * 100) / (carbTar * 1.5))),
                PercentageCarbCaloriesConsumed = Math.Round((dct.TotalDailyCarbCalories + (mealCarb * 4)) * 100.0 / carbTar),
                //CurrentDayCarbCaloriesPercent = Math.Round(((dct.TotalDailyCarbCalories * 100) / (targetTotalCal * 1.5 / 2))),
                MealCarbCalories = (mealCarb * 4),
                MealCarbCaloriesPercent = ((mealCarb * 4 * 100) / (carbTar * 1.5))
                //MealCarbCaloriesPercent = ((mealCarb * 4 * 100) / (targetTotalCal * 1.5 / 2))
            };


            //var showMacros = _context.UserGoals
            //    .Where(g => g.Id == user.Id)
            //    .FirstOrDefault();

            if (showMacros != null)
            {
                cmvm.ShowMacros = true;
            }

            if (mealId != 0)
            {
                cmvm.EditingMeal = true;
            }

            if (dct.TotalDailyCalories > targetTotalCal)
            {
                cmvm.TotalCaloriesSurplusPercent = Math.Round(((dct.TotalDailyCalories + mealCalories) / (double)targetTotalCal) * 100);
                cmvm.TotalCaloriesSurplusDisplayPercent = Math.Round(((dct.TotalDailyCalories + cmvm.MealCalories - cmvm.TargetCalories) * 100) / ((double)targetTotalCal * 1.5));
                cmvm.TotalOldCaloriesSurplusDisplayPercent = Math.Round(((dct.TotalDailyCalories - cmvm.TargetCalories) * 100) / ((double)targetTotalCal * 1.5));
                cmvm.TotalNewCaloriesSurplusDisplayPercent = Math.Round(((cmvm.MealCalories) * 100) / ((double)targetTotalCal * 1.5));
                if (cmvm.TotalOldCaloriesSurplusDisplayPercent > 38)
                {
                    cmvm.TotalOldCaloriesSurplusDisplayPercent = 38;
                    cmvm.TotalNewCaloriesSurplusDisplayPercent = 0;
                }
                else if (cmvm.TotalNewCaloriesSurplusDisplayPercent + cmvm.TotalOldCaloriesSurplusDisplayPercent > 38)
                {
                    cmvm.TotalNewCaloriesSurplusDisplayPercent = 38 - cmvm.TotalOldCaloriesSurplusDisplayPercent;
                }
            }
            else if (dct.TotalDailyCalories + mealCalories > targetTotalCal)
            {
                cmvm.MealCaloriesSurplus = dct.TotalDailyCalories + mealCalories - targetTotalCal;
                cmvm.MealCaloriesSurplusUnderPercent = Math.Round((((mealCalories - cmvm.MealCaloriesSurplus) * 100) / (targetTotalCal * 1.5)));
                cmvm.MealCaloriesSurplusOverPercent = Math.Round(((cmvm.MealCaloriesSurplus * 100) / (targetTotalCal * 1.5)));
                cmvm.MealCaloriesSurplusOverDisplayPercent = Math.Round(100 + (cmvm.MealCaloriesSurplus * 100) / ((double)targetTotalCal));
            }
            else
            {
                cmvm.MealCaloriesRemainingPercent = (67 - (cmvm.CurrentDayCaloriesPercent + cmvm.MealCaloriesPercent));
            }

            if (cmvm.CurrentDayProteinCalories > cmvm.TargetProteinCalories)
            {
                cmvm.TotalProteinCaloriesSurplusPercent = Math.Round(((cmvm.CurrentDayProteinCalories + cmvm.MealProteinCalories) / (double)cmvm.TargetProteinCalories) * 100);
                cmvm.TotalProteinCaloriesSurplusDisplayPercent = Math.Round(((cmvm.CurrentDayProteinCalories + cmvm.MealProteinCalories - cmvm.TargetProteinCalories) * 100) / ((double)cmvm.TargetProteinCalories * 1.5));
                cmvm.TotalOldProteinCaloriesSurplusDisplayPercent = Math.Round(((cmvm.CurrentDayProteinCalories - cmvm.TargetProteinCalories) * 100) / ((double)cmvm.TargetProteinCalories * 1.5));
                cmvm.TotalNewProteinCaloriesSurplusDisplayPercent = Math.Round(((cmvm.MealProteinCalories) * 100) / ((double)cmvm.TargetProteinCalories * 1.5));
                if (cmvm.TotalOldProteinCaloriesSurplusDisplayPercent > 38)
                {
                    cmvm.TotalOldProteinCaloriesSurplusDisplayPercent = 38;
                    cmvm.TotalNewProteinCaloriesSurplusDisplayPercent = 0;
                }
                else if (cmvm.TotalNewProteinCaloriesSurplusDisplayPercent + cmvm.TotalOldProteinCaloriesSurplusDisplayPercent > 38)
                {
                    cmvm.TotalNewProteinCaloriesSurplusDisplayPercent = 38 - cmvm.TotalOldProteinCaloriesSurplusDisplayPercent;
                }
            }
            else if (cmvm.CurrentDayProteinCalories + cmvm.MealProteinCalories > cmvm.TargetProteinCalories)
            {
                cmvm.MealProteinCaloriesSurplus = cmvm.CurrentDayProteinCalories + cmvm.MealProteinCalories - cmvm.TargetProteinCalories;
                cmvm.MealProteinCaloriesSurplusUnderPercent = Math.Round((((cmvm.MealProteinCalories - cmvm.MealProteinCaloriesSurplus) * 100) / (cmvm.TargetProteinCalories * 1.5)));
                cmvm.MealProteinCaloriesSurplusOverPercent = Math.Round(((cmvm.MealProteinCaloriesSurplus * 100) / (cmvm.TargetProteinCalories * 1.5)));
                cmvm.MealProteinCaloriesSurplusOverDisplayPercent = Math.Round(100 + (cmvm.MealProteinCaloriesSurplus * 100) / ((double)cmvm.TargetProteinCalories));
            }
            else
            {
                cmvm.MealProteinCaloriesRemainingPercent = (67 - (cmvm.CurrentDayProteinCaloriesPercent + cmvm.MealProteinCaloriesPercent));
            }

            if (cmvm.CurrentDayFatCalories > cmvm.TargetFatCalories)
            {
                cmvm.TotalFatCaloriesSurplusPercent = Math.Round(((cmvm.CurrentDayFatCalories + cmvm.MealFatCalories) / (double)cmvm.TargetFatCalories) * 100);
                cmvm.TotalFatCaloriesSurplusDisplayPercent = Math.Round(((cmvm.CurrentDayFatCalories + cmvm.MealFatCalories - cmvm.TargetFatCalories) * 100) / ((double)cmvm.TargetFatCalories * 1.5));
                cmvm.TotalOldFatCaloriesSurplusDisplayPercent = Math.Round(((cmvm.CurrentDayFatCalories - cmvm.TargetFatCalories) * 100) / ((double)cmvm.TargetFatCalories * 1.5));
                cmvm.TotalNewFatCaloriesSurplusDisplayPercent = Math.Round(((cmvm.MealFatCalories) * 100) / ((double)cmvm.TargetFatCalories * 1.5));
                if (cmvm.TotalOldFatCaloriesSurplusDisplayPercent > 38)
                {
                    cmvm.TotalOldFatCaloriesSurplusDisplayPercent = 38;
                    cmvm.TotalNewFatCaloriesSurplusDisplayPercent = 0;
                }
                else if (cmvm.TotalNewFatCaloriesSurplusDisplayPercent + cmvm.TotalOldFatCaloriesSurplusDisplayPercent > 38)
                {
                    cmvm.TotalNewFatCaloriesSurplusDisplayPercent = 38 - cmvm.TotalOldFatCaloriesSurplusDisplayPercent;
                }
            }
            else if (cmvm.CurrentDayFatCalories + cmvm.MealFatCalories > cmvm.TargetFatCalories)
            {
                cmvm.MealFatCaloriesSurplus = cmvm.CurrentDayFatCalories + cmvm.MealFatCalories - cmvm.TargetFatCalories;
                cmvm.MealFatCaloriesSurplusUnderPercent = Math.Round((((cmvm.MealFatCalories - cmvm.MealFatCaloriesSurplus) * 100) / (cmvm.TargetFatCalories * 1.5)));
                cmvm.MealFatCaloriesSurplusOverPercent = Math.Round(((cmvm.MealFatCaloriesSurplus * 100) / (cmvm.TargetFatCalories * 1.5)));
                cmvm.MealFatCaloriesSurplusOverDisplayPercent = Math.Round(100 + (cmvm.MealFatCaloriesSurplus * 100) / ((double)cmvm.TargetFatCalories));
            }
            else
            {
                cmvm.MealFatCaloriesRemainingPercent = (67 - (cmvm.CurrentDayFatCaloriesPercent + cmvm.MealFatCaloriesPercent));
            }

            if (cmvm.CurrentDayCarbCalories > cmvm.TargetCarbCalories)
            {
                cmvm.TotalCarbCaloriesSurplusPercent = Math.Round(((cmvm.CurrentDayCarbCalories + cmvm.MealCarbCalories) / (double)cmvm.TargetCarbCalories) * 100);
                cmvm.TotalCarbCaloriesSurplusDisplayPercent = Math.Round(((cmvm.CurrentDayCarbCalories + cmvm.MealCarbCalories - cmvm.TargetCarbCalories) * 100) / ((double)cmvm.TargetCarbCalories * 1.5));
                cmvm.TotalOldCarbCaloriesSurplusDisplayPercent = Math.Round(((cmvm.CurrentDayCarbCalories - cmvm.TargetCarbCalories) * 100) / ((double)cmvm.TargetCarbCalories * 1.5));
                cmvm.TotalNewCarbCaloriesSurplusDisplayPercent = Math.Round(((cmvm.MealCarbCalories) * 100) / ((double)cmvm.TargetCarbCalories * 1.5));
                if (cmvm.TotalOldCarbCaloriesSurplusDisplayPercent > 38)
                {
                    cmvm.TotalOldCarbCaloriesSurplusDisplayPercent = 38;
                    cmvm.TotalNewCarbCaloriesSurplusDisplayPercent = 0;
                }
                else if (cmvm.TotalNewCarbCaloriesSurplusDisplayPercent + cmvm.TotalOldCarbCaloriesSurplusDisplayPercent > 38)
                {
                    cmvm.TotalNewCarbCaloriesSurplusDisplayPercent = 38 - cmvm.TotalOldCarbCaloriesSurplusDisplayPercent;
                }
            }
            else if (cmvm.CurrentDayCarbCalories + cmvm.MealCarbCalories > cmvm.TargetCarbCalories)
            {
                cmvm.MealCarbCaloriesSurplus = cmvm.CurrentDayCarbCalories + cmvm.MealCarbCalories - cmvm.TargetCarbCalories;
                cmvm.MealCarbCaloriesSurplusUnderPercent = Math.Round((((cmvm.MealCarbCalories - cmvm.MealCarbCaloriesSurplus) * 100) / (cmvm.TargetCarbCalories * 1.5)));
                cmvm.MealCarbCaloriesSurplusOverPercent = Math.Round(((cmvm.MealCarbCaloriesSurplus * 100) / (cmvm.TargetCarbCalories * 1.5)));
                cmvm.MealCarbCaloriesSurplusOverDisplayPercent = Math.Round(100 + (cmvm.MealCarbCaloriesSurplus * 100) / ((double)cmvm.TargetCarbCalories));
            }
            else
            {
                cmvm.MealCarbCaloriesRemainingPercent = (67 - (cmvm.CurrentDayCarbCaloriesPercent + cmvm.MealCarbCaloriesPercent));
            }

            cmvm.UserEmail = user.Email;

            if (admin)
            {
                cmvm.Admin = true;
            }

            return cmvm;
        }

        public void UpdateDailyCalories(string userId, DateTime date)
        {

            int dailyCals = _context.Meal
                .Where(x => (x.UserId == userId) && (x.MealDate.Date == date.Date))
                .Sum(x => x.Calories);

            int dailyProteinCals = _context.Meal
                .Where(x => (x.UserId == userId) && (x.MealDate.Date == date.Date))
                .Sum(x => x.ProteinCalories);

            int dailyFatCals = _context.Meal
                .Where(x => (x.UserId == userId) && (x.MealDate.Date == date.Date))
                .Sum(x => x.FatCalories);

            int dailyCarbCals = _context.Meal
                .Where(x => (x.UserId == userId) && (x.MealDate.Date == date.Date))
                .Sum(x => x.CarbCalories);

            int targetTotalCal = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.DailyTargetCalories)
                .FirstOrDefault();

            var existingDayCal = _context.DailyCalTotal
                .Where(x => (x.CalorieDay == date.Date) && (x.Id == userId))
                .FirstOrDefault();

            if (existingDayCal != null)
            {
                existingDayCal.TotalDailyCalories = dailyCals;
                existingDayCal.TotalDailyProteinCalories = dailyProteinCals;
                existingDayCal.TotalDailyFatCalories = dailyFatCals;
                existingDayCal.TotalDailyCarbCalories = dailyCarbCals;

                _context.DailyCalTotal.Update(existingDayCal);

                _context.SaveChanges();
            }
            else
            {
                DailyCalTotal dct = new DailyCalTotal()
                {
                    TotalDailyCalories = dailyCals,
                    TargetDailyCalories = targetTotalCal,
                    TotalDailyProteinCalories = dailyProteinCals,
                    TotalDailyFatCalories = dailyFatCals,
                    TotalDailyCarbCalories = dailyCarbCals,
                    Id = userId,
                    CalorieDay = date
                };


                _context.DailyCalTotal.Add(dct);

                _context.SaveChanges();
            }
        }

        
    }
}
