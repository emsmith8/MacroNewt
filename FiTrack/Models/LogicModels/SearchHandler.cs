using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FiTrack.Models.ViewModels;
using FiTrack.Models.Type;
using System.Net.Http;

namespace FiTrack.Models.LogicModels
{
    public class SearchHandler
    {

        public string OrganizeSearchQ(string foodName)
        {
            string urlParameters = "search/?format=json&q=" + foodName + "&ds=Standard+Reference&sort=n&max=1500&offset=0&api_key=5jOuzAkdWfOOH2x5yPgd2oWsyzGVkyrrkElAMSsl";

            return urlParameters;
        }


        public string OrganizeReportQ(string foodNdbno)
        {
            string urlParameters = "reports/?format=json&ndbno=" + foodNdbno + "&type=b&api_key=5jOuzAkdWfOOH2x5yPgd2oWsyzGVkyrrkElAMSsl";
            
            return urlParameters;
        }


        public FoodViewModel StoreSearchReturns(String APIData)
        {
            JObject joResponse = JObject.Parse(APIData);
            JObject ojObject = (JObject)joResponse["list"];
            JArray array = (JArray)ojObject["item"];
            
            List<FoodListItem> availableFoods = JsonConvert.DeserializeObject<List<FoodListItem>>(array.ToString());

            FoodViewModel fvm = new FoodViewModel()
            {
                Foods = availableFoods
            };

            return fvm;
        }

        public List<FoodListItem> AddNutrientValue(String APIData, List<FoodListItem> foodList)
        {
            JObject joResponse = JObject.Parse(APIData);

            JObject ojObject = (JObject)joResponse["report"];

            JValue targetFood = (JValue)ojObject["food"]["ndbno"];

            JArray array = (JArray)ojObject["food"]["nutrients"];

            String nutValue = "";
            String nutUnit = "";

            foreach (FoodListItem fli in foodList)
            {
                if (fli.Ndbno == targetFood.ToString())
                {

                    foreach (var item in array.Children())
                    {
                        //          JValue targetNutrient = (JValue)item["name"];
                        JValue targetNutrient = (JValue)item["name"];

                        Debug.WriteLine("The targetNutrient is " + targetNutrient.ToString());

                        if (targetNutrient.ToString() == "Energy")
                        {
                            nutValue = item["value"].ToString();
                            nutUnit = item["unit"].ToString();
                            Debug.WriteLine("The value is " + nutValue + " and the unit is " + nutUnit);
                        }
                    }

                    fli.Value = nutValue;
                    fli.Unit = nutUnit;
                }
                
                
            }

            return foodList;
        }
/*
        public FoodNutrientViewModel StoreReportReturns(String APIData, List<FoodListItem> foodList)
        {
            JObject joResponse = JObject.Parse(APIData);

            JObject ojObject = (JObject)joResponse["report"];
            JArray array = (JArray)ojObject["food"]["nutrients"];

            foreach (FoodListItem fli in foodList)
            {
                var itemProperties = array.Children<JProperty>();
                var itemElement = itemProperties.FirstOrDefault(x => x.Name == "Engery");
                foreach (var item in array.Children())
                {
                    var itemProperties = item.Children<JProperty>();
                    var myElement = itemProperties.FirstOrDefault(x => x.Name == "Engery");
                }
            }

            List<FoodNutrientsItem> foodNutrients = JsonConvert.DeserializeObject<List<FoodNutrientsItem>>(array.ToString());
            //       List<FoodNutrientsItem> foodNutrients = JsonConvert.DeserializeObject<List<FoodNutrientsItem>>(array.ToString());

            

            FoodNutrientViewModel fnvm = new FoodNutrientViewModel()
            {
                Foods = foodNutrients
            };
            

            return fnvm;
        }
*/

    }
}
