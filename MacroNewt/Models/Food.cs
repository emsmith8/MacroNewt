using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MacroNewt.Models
{
    public class Food
    {
        [Key]
        public int FoodId { get; set; }
        public string Name { get; set; }
        [Required]
        public string Ndbno { get; set; }
        public List<Nutrient> Nutrients { get; set; }
        public string Unit { get; set; }
        [Required(ErrorMessage = "You must select a portion size")]        
        public int? Value { get; set; }
        [Range(0.25, 10.0, ErrorMessage = "Must be between 0.25 and 10")]
        public decimal NumberOfServings {get;set;}
        public int PortionIndex { get; set; }       

        public string SelectedPortionLabel { get; set; }
        public string SelectedPortionQty { get; set; }

        public string FoodTotalProtein { get; set; }
        public string FoodTotalFat { get; set; }
        public string FoodTotalCarb { get; set; }

        public int MealId {get; set;}
        public virtual Meal Meal { get; set; }

        public Food()
        {
            Nutrients = new List<Nutrient>();
        }

        public Measure SelectedMeasure()
        {
            if ((Nutrients != null && Nutrients.Count > 0) && PortionIndex != 0)
            {
                int measureIndex = PortionIndex -1;

                Measure whatever;

                if (SelectedPortionLabel == "100 g")
                {
                    whatever = null;
                }
                else
                {
                    whatever = Nutrients.SelectMany(s => s.Measures)
                                    .Where(f => (f.Label == SelectedPortionLabel) && (f.Qty == SelectedPortionQty)).First();
                }

                
                                    

                return whatever;
            }
            else
            {
                return null;
            }

        }

        public double GetCalories(string type)
        {
            if (Nutrients == null || Nutrients.Count == 0)
            {
                return 0;
            }
            else
            {
                Nutrient calories = Nutrients.Where(x => x.NId == "208").FirstOrDefault();
                if (calories == null)
                {
                    return 0;
                }
                else
                {
                    if (type == "regular")
                    {
                        return Convert.ToDouble(calories.Measures.Where(x => (x.Label == SelectedPortionLabel) && (x.Qty == SelectedPortionQty)).FirstOrDefault().Value * NumberOfServings);
                    }
                    else if (type == "explore" || type == "noMeasure")
                    {
                        return Convert.ToDouble(calories.Value);
                    }
                    else
                    {
                        return 0;
                    }
                
                }
            }
        }
        

        public double GetNutrientValue(string nutrientId, string type)
        {
            if (Nutrients == null || Nutrients.Count == 0)
            {
                return 0;
            }
            else
            {
                Nutrient nutrient = Nutrients.Where(x => x.NId == nutrientId).FirstOrDefault();
                if (nutrient == null)
                {
                    return 0;
                }
                else
                {
                    if (type == "regular")
                    {
                        return Convert.ToDouble(nutrient.Measures.Where(x => (x.Label == SelectedPortionLabel) && (x.Qty == SelectedPortionQty)).FirstOrDefault().Value * NumberOfServings);
                    }
                    else if (type == "explore" || type == "noMeasure")
                    {
                        return Convert.ToDouble(nutrient.Value);
                    }
                    else
                    {
                        return 0;
                    }
                   
                }
            }
        }
        
        public string GetNutrientDisplayValue(string nutrientId, string type)
        {
            if (Nutrients == null || Nutrients.Count == 0)
            {
                return "";
            }
            else
            {
                Nutrient nutrient = Nutrients.Where(x => x.NId == nutrientId).FirstOrDefault();
                if (nutrient == null)
                {
                    return "";
                }
                else
                {
                    if (type == "regular")
                    {
                        return $"{String.Format("{0:0.##}", (nutrient.Measures.Where(x => (x.Label == SelectedPortionLabel) && (x.Qty == SelectedPortionQty)).FirstOrDefault().Value * NumberOfServings))} {nutrient.Unit}";
                    }
                    else if (type == "explore")
                    {
                        return $"{String.Format("{0:0.##}", nutrient.Value)} {nutrient.Unit}";
                    }
                    else
                    {
                        return "";
                    }
                    
                }
            }
        }

        public static NoMeasureFood GetNoMeasureNutrients(Food food)
        {
            NoMeasureFood nmf = new NoMeasureFood();

            nmf.FoodId = food.FoodId;
            nmf.Mass = 100 * (double)food.NumberOfServings;
            nmf.Calories = food.GetCalories("noMeasure") * (double)food.NumberOfServings;
            nmf.Fat = food.GetNutrientValue("204", "noMeasure") * (double)food.NumberOfServings;
            nmf.TransFat = food.GetNutrientValue("605", "noMeasure") * (double)food.NumberOfServings;
            nmf.SatFat = food.GetNutrientValue("606", "noMeasure") * (double)food.NumberOfServings;
            nmf.PolyFat = food.GetNutrientValue("646", "noMeasure") * (double)food.NumberOfServings;
            nmf.MonoFat = food.GetNutrientValue("645", "noMeasure") * (double)food.NumberOfServings;
            nmf.Cholesterol = food.GetNutrientValue("601", "noMeasure") * (double)food.NumberOfServings;
            nmf.Sodium = food.GetNutrientValue("307", "noMeasure") * (double)food.NumberOfServings;
            nmf.Carbs = food.GetNutrientValue("205", "noMeasure") * (double)food.NumberOfServings;
            nmf.Fiber = food.GetNutrientValue("291", "noMeasure") * (double)food.NumberOfServings;
            nmf.Sugar = food.GetNutrientValue("269", "noMeasure") * (double)food.NumberOfServings;
            nmf.Protein = food.GetNutrientValue("203", "noMeasure") * (double)food.NumberOfServings;
            nmf.VitA = food.GetNutrientValue("320", "noMeasure") * (double)food.NumberOfServings;
            nmf.VitC = food.GetNutrientValue("401", "noMeasure") * (double)food.NumberOfServings;
            nmf.Calcium = food.GetNutrientValue("301", "noMeasure") * (double)food.NumberOfServings;
            nmf.Iron = food.GetNutrientValue("303", "noMeasure") * (double)food.NumberOfServings;

            return nmf;
        }

        public static MealTotalNutrient GetTotalMealNutrients(List<Food> foods)
        {
            MealTotalNutrient mt = new MealTotalNutrient();

            foreach (Food f in foods)
            {
                if (f.Nutrients[0].Measures.Count == 0)
                {
                    mt.Mass = 100 * (double)f.NumberOfServings;
                    mt.Calories = f.GetCalories("noMeasure") * (double)f.NumberOfServings;
                    mt.Fat = f.GetNutrientValue("204", "noMeasure") * (double)f.NumberOfServings;
                    mt.TransFat = f.GetNutrientValue("605", "noMeasure") * (double)f.NumberOfServings;
                    mt.SatFat = f.GetNutrientValue("606", "noMeasure") * (double)f.NumberOfServings;
                    mt.PolyFat = f.GetNutrientValue("646", "noMeasure") * (double)f.NumberOfServings;
                    mt.MonoFat = f.GetNutrientValue("645", "noMeasure") * (double)f.NumberOfServings;
                    mt.Cholesterol = f.GetNutrientValue("601", "noMeasure") * (double)f.NumberOfServings;
                    mt.Sodium = f.GetNutrientValue("307", "noMeasure") * (double)f.NumberOfServings;
                    mt.Carbs = f.GetNutrientValue("205", "noMeasure") * (double)f.NumberOfServings;
                    mt.Fiber = f.GetNutrientValue("291", "noMeasure") * (double)f.NumberOfServings;
                    mt.Sugar = f.GetNutrientValue("269", "noMeasure") * (double)f.NumberOfServings;
                    mt.Protein = f.GetNutrientValue("203", "noMeasure") * (double)f.NumberOfServings;
                    mt.VitA = f.GetNutrientValue("320", "noMeasure") * (double)f.NumberOfServings;
                    mt.VitC = f.GetNutrientValue("401", "noMeasure") * (double)f.NumberOfServings;
                    mt.Calcium = f.GetNutrientValue("301", "noMeasure") * (double)f.NumberOfServings;
                    mt.Iron = f.GetNutrientValue("303", "noMeasure") * (double)f.NumberOfServings;
                }
                else
                {
                    mt.Mass += Convert.ToDouble(f.Nutrients[0].Measures[0].Eqv) * (double)f.NumberOfServings;
                    mt.Calories += f.GetCalories("regular");
                    mt.Fat += f.GetNutrientValue("204", "regular");
                    mt.TransFat += f.GetNutrientValue("605", "regular");
                    mt.SatFat += f.GetNutrientValue("606", "regular");
                    mt.PolyFat += f.GetNutrientValue("646", "regular");
                    mt.MonoFat += f.GetNutrientValue("645", "regular");
                    mt.Cholesterol += f.GetNutrientValue("601", "regular");
                    mt.Sodium += f.GetNutrientValue("307", "regular");
                    mt.Carbs += f.GetNutrientValue("205", "regular");
                    mt.Fiber += f.GetNutrientValue("291", "regular");
                    mt.Sugar += f.GetNutrientValue("269", "regular");
                    mt.Protein += f.GetNutrientValue("203", "regular");
                    mt.VitA += f.GetNutrientValue("320", "regular");
                    mt.VitC += f.GetNutrientValue("401", "regular");
                    mt.Calcium += f.GetNutrientValue("301", "regular");
                    mt.Iron += f.GetNutrientValue("303", "regular");
                }
            }

            return mt;

        }


    }
    
    
    public class Nutrient
    {
        [Key]
        public int NutrientId { get; set; }
        public string NId { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Unit { get; set; }
        public decimal Value { get; set; }
        public List<Measure> Measures { get; set; }

        public int FoodId { get; set; }
        public virtual Food Food { get; set; }

        public Nutrient()
        {
            Measures = new List<Measure>();
        }
    }

    public class Measure
    {
        [Key]
        public int MeasureId { get; set; }
        public string Label { get; set; }
        public string Eqv { get; set; }
        public string Eunit { get; set; }
        public string Qty { get; set; }
        public decimal Value { get; set; }
        public string DisplayName { get; set; }

        public int NutrientId { get; set; }
        public virtual Nutrient Nutrient { get; set; }
           
    }

    public class MealTotalNutrient
    {
        public double Mass { get; set; }
        public double Calories { get; set; }
        public double Fat { get; set; }
        public double TransFat { get; set; }
        public double SatFat { get; set; }
        public double PolyFat { get; set; }
        public double MonoFat { get; set; }
        public double Cholesterol { get; set; }
        public double Sodium { get; set; }
        public double Carbs { get; set; }
        public double Fiber { get; set; }
        public double Sugar { get; set; }
        public double Protein { get; set; }
        public double VitA { get; set; }
        public double VitC { get; set; }
        public double Calcium { get; set; }
        public double Iron { get; set; }
    }

    public class NoMeasureFood
    {
        public int FoodId { get; set; }
        public double Mass { get; set; }
        public double Calories { get; set; }
        public double Fat { get; set; }
        public double TransFat { get; set; }
        public double SatFat { get; set; }
        public double PolyFat { get; set; }
        public double MonoFat { get; set; }
        public double Cholesterol { get; set; }
        public double Sodium { get; set; }
        public double Carbs { get; set; }
        public double Fiber { get; set; }
        public double Sugar { get; set; }
        public double Protein { get; set; }
        public double VitA { get; set; }
        public double VitC { get; set; }
        public double Calcium { get; set; }
        public double Iron { get; set; }
    }

}
