using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MacroNewt.Models.LogicModels
{
    /*
     *  The SearchHandler class
     *  Handles the semantics of sending API calls and properly organizing and storing response data.
     *  ** This class will need to change when the USDA API is permanently migrated to new database **
     */

    /// <summary>
    /// This class is used to organize API calls and store responses
    /// </summary>
    /// <remarks>
    /// Searching for foods and requesting additional information are separate API calls thus require different string parameters.
    /// </remarks>
    public class SearchHandler
    {
        /// <summary>
        /// Combines the provided food name with the required syntax for an API food search of the specified database type
        /// </summary>
        /// <param name="foodName"></param>
        /// <param name="database"></param>
        /// <returns>The final string for API search request</returns>
        public string OrganizeSearchQ(string foodName, string database)
        {
            string urlParameters;

            if (database == "Branded")
            {
                urlParameters = "search/?format=json&q=" + foodName + "&ds=Branded+Food+Products&sort=n&max=1500&offset=0&api_key=5jOuzAkdWfOOH2x5yPgd2oWsyzGVkyrrkElAMSsl";
            }
            else
            {
                urlParameters = "search/?format=json&q=" + foodName + "&ds=Standard+Reference&sort=n&max=1500&offset=0&api_key=5jOuzAkdWfOOH2x5yPgd2oWsyzGVkyrrkElAMSsl";
            }


            return urlParameters;
        }

        /// <summary>
        /// Combines the provided food ndbno identifier with the required syntax for an API food report
        /// </summary>
        /// <param name="foodNdbno"></param>
        /// <returns>The final string for API report request</returns>
        public string OrganizeReportQ(string foodNdbno)
        {
            string urlParameters = "reports/?format=json&ndbno=" + foodNdbno + "&type=b&api_key=5jOuzAkdWfOOH2x5yPgd2oWsyzGVkyrrkElAMSsl";

            return urlParameters;
        }


        /// <summary>
        /// Converts the string response from API food search into a list of <see cref="Food"/> objects
        /// </summary>
        /// <param name="APIData"></param>
        /// <returns>A list of food objects from API response</returns>
        public List<Food> StoreSearchReturns(string APIData)
        {
            JObject joResponse = JObject.Parse(APIData);
            JObject ojObject = (JObject)joResponse["list"];
            JArray array = (JArray)ojObject["item"];

            List<Food> availableFoods = JsonConvert.DeserializeObject<List<Food>>(array.ToString());

            return availableFoods;
        }

        /// <summary>
        /// Parses and stores both the <see cref="Nutrient"/> and <see cref="Measure"/> details of a food from API response
        /// </summary>
        /// <remarks>
        /// Food nutrient and measure details aren't available in food searches, only in specific food report requests from API.
        ///     To avoid unnecessary overhead, details are only obtained for the foods specifically selected during meal logging.
        /// </remarks>
        /// <param name="f"></param>
        /// <param name="dataObject"></param>
        /// <param name="detailType"></param>
        /// <returns>A <see cref="Food"/> object with details included</returns>
        public Food StoreMealNutrientDetails(Food f, JObject dataObject, string detailType)
        {
            f.Ndbno = dataObject["report"]["food"]["ndbno"].ToString();
            f.Name = dataObject["report"]["food"]["name"].ToString();
            f.Unit = dataObject["report"]["food"]["ru"].ToString();

            var nutrients = dataObject["report"]["food"]["nutrients"];

            foreach (var item in nutrients)
            {
                if (detailType == "simple")
                {
                    if ((item["nutrient_id"].ToString() == "208")
                    || (item["nutrient_id"].ToString() == "203")
                    || (item["nutrient_id"].ToString() == "204")
                    || (item["nutrient_id"].ToString() == "205")
                    )
                    {
                        Nutrient n = new Nutrient()
                        {
                            NId = item["nutrient_id"].ToString(),
                            Name = item["name"].ToString(),
                            Group = item["group"].ToString(),
                            Unit = item["unit"].ToString(),
                            Value = Convert.ToDecimal(item["value"])
                        };

                        var measures = item["measures"];

                        foreach (var meas in measures)
                        {
                            Measure m = new Measure()
                            {
                                Label = meas["label"].ToString(),
                                Eqv = meas["eqv"].ToString(),
                                Eunit = meas["eunit"].ToString(),
                                Qty = meas["qty"].ToString(),
                                Value = Convert.ToDecimal(meas["value"]),
                                DisplayName = $"{meas["qty"].ToString()} {meas["label"].ToString()} ({meas["value"].ToString()} {n.Unit})"
                            };
                            n.Measures.Add(m);
                        }
                        f.Nutrients.Add(n);
                    }
                }
                else
                {
                    Nutrient n = new Nutrient()
                    {
                        NId = item["nutrient_id"].ToString(),
                        Name = item["name"].ToString(),
                        Group = item["group"].ToString(),
                        Unit = item["unit"].ToString(),
                        Value = Convert.ToDecimal(item["value"])
                    };

                    var measures = item["measures"];

                    foreach (var meas in measures)
                    {
                        Measure m = new Measure()
                        {
                            Label = meas["label"].ToString(),
                            Eqv = meas["eqv"].ToString(),
                            Eunit = meas["eunit"].ToString(),
                            Qty = meas["qty"].ToString(),
                            Value = Convert.ToDecimal(meas["value"]),
                            DisplayName = $"{meas["qty"].ToString()} {meas["label"].ToString()} ({meas["value"].ToString()} {n.Unit})"
                        };
                        n.Measures.Add(m);
                    }
                    f.Nutrients.Add(n);
                }


            }

            return f;
        }

    }
}
