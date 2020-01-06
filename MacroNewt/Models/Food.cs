using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MacroNewt.Models
{
    /*
     *  The Food class
     *  Has public members for all relevant food information
     */

    /// <summary>
    /// A Food object includes all information relevant to a paticular food, including a list of <see cref="Nutrient"/> in that food
    /// </summary>
    /// <remarks>
    /// Relevant information includes identifiers like name and database NDBNO, user selected portion and number of servings, and overall macronutrient contents
    /// </remarks>
    public class Food
    {
        /// <summary>
        /// Main identifier for a Food in the database
        /// </summary>
        [Key]
        public int FoodId { get; set; }
        /// <summary>
        /// The food's name as supplied by the USDA database.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The identifier used by the USDA database. Necessary for API food detail calls.
        /// </summary>
        [Required]
        public string Ndbno { get; set; }
        /// <summary>
        /// A list of all <see cref="Nutrient"/> items contained in the Food. Used for tracking macro progress.
        /// </summary>
        public List<Nutrient> Nutrients { get; set; }
        /// <summary>
        /// The unit used for calculating nutrient content without serving sizes or portions.
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// The calorie value of the food given selected portion size and number of servings.
        /// </summary>
        [Required(ErrorMessage = "You must select a portion size")]
        public int? Value { get; set; }
        /// <summary>
        /// The user-chosen number of servings of a selected portion of food.
        /// </summary>
        [Range(0.25, 100.0, ErrorMessage = "Must be between 0.25 and 100")]
        public decimal NumberOfServings { get; set; }
        /// <summary>
        /// An index value used to store which of the available portion choices was selected.
        /// </summary>
        public int PortionIndex { get; set; }
        /// <summary>
        /// The label assigned to the selected food portion (i.e. 'serving', 'tbsp').
        /// </summary>
        public string SelectedPortionLabel { get; set; }
        /// <summary>
        /// The user-selected number of servings of a given portion of food.
        /// </summary>
        public string SelectedPortionQty { get; set; }
        /// <summary>
        /// Stores the total protein content of selected food based on portion and number of servings.
        /// </summary>
        public string FoodTotalProtein { get; set; }
        /// <summary>
        /// Stores the total fat content of selected food based on portion and number of servings.
        /// </summary>
        public string FoodTotalFat { get; set; }
        /// <summary>
        /// Stores the total carb content of selected food based on portion and number of servings.
        /// </summary>
        public string FoodTotalCarb { get; set; }
        /// <summary>
        /// Identifier for the associated meal of this food in its given portion and quantity.
        /// </summary>
        public int MealId { get; set; }
        /// <summary>
        /// Virtual property to allow for EF lazy loading of associated <see cref="Meal"/> record.
        /// </summary>
        public virtual Meal Meal { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Food"/> class.
        /// </summary>
        public Food()
        {
            Nutrients = new List<Nutrient>();
        }

        /// <summary>
        /// Used to determine the variable measure label of a selected food and create a meaningful object as a result
        /// </summary>
        /// <returns>A <see cref="Measure"/> object</returns>
        public Measure SelectedMeasure()
        {
            if ((Nutrients != null && Nutrients.Count > 0) && PortionIndex != 0)
            {
                int measureIndex = PortionIndex - 1;

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

        /// <summary>
        /// Used to determine and retrieve the calories in a food based on portion type and quantity
        /// </summary>
        /// <param name="type">A string representation of the detail expected in <see cref="Nutrient"/> record</param>
        /// <returns>A double value representing total food calories</returns>
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

        /// <summary>
        /// Used to determine and retrieve the specified nutrient content in a food based on target nutrient, portion type, and quantity
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="GetNutrientDisplayValue(string, string)"/>, this method is used during the meal logging process
        /// </remarks>
        /// <param name="nutrientId">The string identifier of the target <see cref="Nutrient"/> record</param>
        /// <param name="type">A string representation of the detail expected in <see cref="Nutrient"/> record</param>
        /// <returns>A double value representing total content of specified nutrient</returns>
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

        /// <summary>
        /// Returns a string of nutrient content information for display purposes
        /// </summary>
        /// <remarks>
        /// Simliar to <see cref="GetNutrientValue(string, string)"/>, but instead of returning a double this method returns a string for display in nutrition labels
        /// </remarks>
        /// <param name="nutrientId">The string identifier of the target <see cref="Nutrient"/> record</param>
        /// <param name="type">A string representation of the detail expected in <see cref="Nutrient"/> record</param>
        /// <returns>A string representinng specified nutrition content</returns>
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

        /// <summary>
        /// Used to retrive <see cref="Nutrient"/> information for a food when a measure isn't available
        /// </summary>
        /// <remarks>
        /// Some foods in the database don't include <see cref="Measure"/>, which breaks other code. This method handles foods without measures.
        /// </remarks>
        /// <param name="food">The <see cref="Food"/> object for which nutritional data is being displayed, but without any associated <see cref="Measure"/></param>
        /// <returns>A populated <see cref="NoMeasureFood"/> object</returns>
        public static NoMeasureFood GetNoMeasureNutrients(Food food)
        {
            NoMeasureFood nmf = new NoMeasureFood
            {
                FoodId = food.FoodId,
                Mass = 100 * (double)food.NumberOfServings,
                Calories = food.GetCalories("noMeasure") * (double)food.NumberOfServings,
                Fat = food.GetNutrientValue("204", "noMeasure") * (double)food.NumberOfServings,
                TransFat = food.GetNutrientValue("605", "noMeasure") * (double)food.NumberOfServings,
                SatFat = food.GetNutrientValue("606", "noMeasure") * (double)food.NumberOfServings,
                PolyFat = food.GetNutrientValue("646", "noMeasure") * (double)food.NumberOfServings,
                MonoFat = food.GetNutrientValue("645", "noMeasure") * (double)food.NumberOfServings,
                Cholesterol = food.GetNutrientValue("601", "noMeasure") * (double)food.NumberOfServings,
                Sodium = food.GetNutrientValue("307", "noMeasure") * (double)food.NumberOfServings,
                Carbs = food.GetNutrientValue("205", "noMeasure") * (double)food.NumberOfServings,
                Fiber = food.GetNutrientValue("291", "noMeasure") * (double)food.NumberOfServings,
                Sugar = food.GetNutrientValue("269", "noMeasure") * (double)food.NumberOfServings,
                Protein = food.GetNutrientValue("203", "noMeasure") * (double)food.NumberOfServings,
                VitA = food.GetNutrientValue("320", "noMeasure") * (double)food.NumberOfServings,
                VitC = food.GetNutrientValue("401", "noMeasure") * (double)food.NumberOfServings,
                Calcium = food.GetNutrientValue("301", "noMeasure") * (double)food.NumberOfServings,
                Iron = food.GetNutrientValue("303", "noMeasure") * (double)food.NumberOfServings
            };

            return nmf;
        }

        /// <summary>
        /// Used in the creation of nutrition labels for complete meals
        /// </summary>
        /// <remarks>
        /// The method loops through each food included in a meal and combines all calorie and nutrient information in order to 
        ///     provide a nutrition label for entire meal.
        /// </remarks>
        /// <param name="foods">A list of <see cref="Food"/> objects for which nutritional data is being displayed</param>
        /// <returns>A populated <see cref="MealTotalNutrient"/> object</returns>
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

    /*
     *  The Nutrient class
     *  Has public members for all nutrient details of a particular food
     */

    /// <summary>
    /// A Nutrient object includes all information relevant to the nutritional data of a particular food, including a list of associated <see cref="Measure"/>
    /// </summary>
    /// <remarks>
    /// Relevant information includes identifiers like name and database NID, value of the particular nutrient, and the <see cref="Food"/> it is associated with
    /// </remarks>
    public class Nutrient
    {
        /// <summary>
        /// The int identifier of the Nutrient
        /// </summary>
        [Key]
        public int NutrientId { get; set; }
        /// <summary>
        /// The USDA database ID for the Nutrient
        /// </summary>
        public string NId { get; set; }
        /// <summary>
        /// The name of the Nutrient
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Nutrient's food type group as established by USDA (e.g. Vitamins, Minerals, Proximates)
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// The base unit of measure (e.g. g, mg, kcal)
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// The value associated with the unit for the selected Nutrient in the given portion/servings of parent Food
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// A list of <see cref="Measure"/> options associated with the Nutrient, for which each have an equivalency multiplier
        /// </summary>
        public List<Measure> Measures { get; set; }
        /// <summary>
        /// The int identifier of the parent <see cref="Food"/> object
        /// </summary>
        public int FoodId { get; set; }
        /// <summary>
        /// Virtual property to allow for EF lazy loading of associated <see cref="Food"/> record
        /// </summary>
        public virtual Food Food { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nutrient"/> class.
        /// </summary>
        public Nutrient()
        {
            Measures = new List<Measure>();
        }
    }

    /*
     *  The Measure class
     *  Has public members for all measure details of a particular nutrient
     */

    /// <summary>
    /// A Measure object includes all information relevant to the portion of a particular <see cref="Food"/>
    /// </summary>
    /// <remarks>
    /// The portion of a Food dictates the values of all nutrients. Relevant information includes identifiers like label and MeasureId, value of
    ///     a particular measure, and the <see cref="Nutrient"/> it is associated with
    /// </remarks>
    public class Measure
    {
        /// <summary>
        /// The int identifier of the Measure
        /// </summary>
        [Key]
        public int MeasureId { get; set; }
        /// <summary>
        /// The quantifying string label of the Measure (e.g. serving, jar, oz)
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// The string gram equivalency of the Measure
        /// </summary>
        public string Eqv { get; set; }
        /// <summary>
        /// The string base conversion unit for Measure equivalencies (gram)
        /// </summary>
        public string Eunit { get; set; }
        /// <summary>
        /// The string selected number of servings of parent Food
        /// </summary>
        public string Qty { get; set; }
        /// <summary>
        /// The decimal value of Nutrient in given Measure
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// A string combination of the quantity and label (e.g. '2 oz')
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// The int identifier of the parent <see cref="Nutrient"/> object
        /// </summary>
        public int NutrientId { get; set; }
        /// <summary>
        /// Virtual property to allow for EF lazy loading of associated <see cref="Nutrient"/> record
        /// </summary>
        public virtual Nutrient Nutrient { get; set; }

    }
    /*
     *  The MealTotalNutrient class
     *  Has public members for all nutrient details of an entire meal
     */

    /// <summary>
    /// A MealTotalNutrient object includes total values of all nutrients from all <see cref="Food"/> items in the meal 
    /// </summary>
    public class MealTotalNutrient
    {
        /// <summary>
        /// Double value of the Meal's total mass
        /// </summary>
        public double Mass { get; set; }
        /// <summary>
        /// Double value of the total calories in the Meal
        /// </summary>
        public double Calories { get; set; }
        /// <summary>
        /// Double value of the total fat in the Meal
        /// </summary>
        public double Fat { get; set; }
        /// <summary>
        /// Double value of the trans fat, if any, in the Meal
        /// </summary>
        public double TransFat { get; set; }
        /// <summary>
        /// Double value of the saturated fat, if any, in the Meal
        /// </summary>
        public double SatFat { get; set; }
        /// <summary>
        /// Double value of the polyunsaturated fat, if any, in the Meal
        /// </summary>
        public double PolyFat { get; set; }
        /// <summary>
        /// Double value of the monounsaturated fat, if any, in the Meal
        /// </summary>
        public double MonoFat { get; set; }
        /// <summary>
        /// Double value of the cholesterol in the Meal
        /// </summary>
        public double Cholesterol { get; set; }
        /// <summary>
        /// Double value of the sodium in the Meal
        /// </summary>
        public double Sodium { get; set; }
        /// <summary>
        /// Double value of the carbohydrates in the Meal
        /// </summary>
        public double Carbs { get; set; }
        /// <summary>
        /// Double value of the fiber in the Meal
        /// </summary>
        public double Fiber { get; set; }
        /// <summary>
        /// Double value of the sugar in the Meal
        /// </summary>
        public double Sugar { get; set; }
        /// <summary>
        /// Double value of the protein in the Meal
        /// </summary>
        public double Protein { get; set; }
        /// <summary>
        /// Double value of the vitramin A in the Meal
        /// </summary>
        public double VitA { get; set; }
        /// <summary>
        /// Double value of the vitamin C in the Meal
        /// </summary>
        public double VitC { get; set; }
        /// <summary>
        /// Double value of the calcium in the Meal
        /// </summary>
        public double Calcium { get; set; }
        /// <summary>
        /// Double value of the iron in the Meal
        /// </summary>
        public double Iron { get; set; }
    }

    /*
     *  The NoMeasureFood class
     *  Has public members for all nutrient details of a food without any associated measures 
     */

    /// <summary>
    /// A NoMeasureFood object includes all nutrient data of a food without having any inherently associated measure
    /// </summary>
    public class NoMeasureFood
    {
        /// <summary>
        /// Int identifier of the <see cref="Food"/> object without measures
        /// </summary>
        public int FoodId { get; set; }
        /// <summary>
        /// Double value of the Foods's total mass
        /// </summary>
        public double Mass { get; set; }
        /// <summary>
        /// Double value of the total calories in the Food
        /// </summary>
        public double Calories { get; set; }
        /// <summary>
        /// Double value of the total fat in the Food
        /// </summary>
        public double Fat { get; set; }
        /// <summary>
        /// Double value of the trans fat, if any, in the Food
        /// </summary>
        public double TransFat { get; set; }
        /// <summary>
        /// Double value of the saturated fat, if any, in the Food
        /// </summary>
        public double SatFat { get; set; }
        /// <summary>
        /// Double value of the polyunsaturated fat, if any, in the Food
        /// </summary>
        public double PolyFat { get; set; }
        /// <summary>
        /// Double value of the monounsaturated fat, if any, in the Food
        /// </summary>
        public double MonoFat { get; set; }
        /// <summary>
        /// Double value of the total cholesterol in the Food
        /// </summary>
        public double Cholesterol { get; set; }
        /// <summary>
        /// Double value of the total sodium in the Food
        /// </summary>
        public double Sodium { get; set; }
        /// <summary>
        /// Double value of the total carbohydrates in the Food
        /// </summary>
        public double Carbs { get; set; }
        /// <summary>
        /// Double value of the total fiber in the Food
        /// </summary>
        public double Fiber { get; set; }
        /// <summary>
        /// Double value of the total sugar in the Food
        /// </summary>
        public double Sugar { get; set; }
        /// <summary>
        /// Double value of the total protein in the Food
        /// </summary>
        public double Protein { get; set; }
        /// <summary>
        /// Double value of the total vitamin A in the Food
        /// </summary>
        public double VitA { get; set; }
        /// <summary>
        /// Double value of the total vitamin C in the Food
        /// </summary>
        public double VitC { get; set; }
        /// <summary>
        /// Double value of the total calcium in the Food
        /// </summary>
        public double Calcium { get; set; }
        /// <summary>
        /// Double value of the total iron in the Food
        /// </summary>
        public double Iron { get; set; }
    }

}
