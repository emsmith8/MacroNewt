using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MacroNewt.Models.ViewModels;
using System.Net.Http;

namespace MacroNewt.Models.LogicModels
{
    public class SearchHandler
    {

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


        public string OrganizeReportQ(string foodNdbno)
        {
            string urlParameters = "reports/?format=json&ndbno=" + foodNdbno + "&type=b&api_key=5jOuzAkdWfOOH2x5yPgd2oWsyzGVkyrrkElAMSsl";
            
            return urlParameters;
        }
        


        public List<Food> StoreSearchReturns(string APIData)
        {
            JObject joResponse = JObject.Parse(APIData);
            JObject ojObject = (JObject)joResponse["list"];
            JArray array = (JArray)ojObject["item"];
            
            List<Food> availableFoods = JsonConvert.DeserializeObject<List<Food>>(array.ToString());
            
            return availableFoods;
        }


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
