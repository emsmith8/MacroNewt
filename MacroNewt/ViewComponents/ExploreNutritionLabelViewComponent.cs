using MacroNewt.Models;
using MacroNewt.Models.LogicModels;
using MacroNewt.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MacroNewt.ViewComponents
{
    public class ExploreNutritionLabelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ExplorePortionChoiceViewModel epc)
        {
            SearchHandler handler = new SearchHandler();

            var client = HttpClientAccessor.HttpClient;

            HttpResponseMessage response = client.GetAsync(handler.OrganizeReportQ(epc.Ndbno)).Result;

            Food f = new Food();

            int targetIndex = epc.PortionIndex;

            if (response.IsSuccessStatusCode)
            {
                JObject dataObject = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                f.Ndbno = dataObject["report"]["food"]["ndbno"].ToString();
                f.Name = dataObject["report"]["food"]["name"].ToString();
                f.Unit = dataObject["report"]["food"]["ru"].ToString();

                

                var nutrients = dataObject["report"]["food"]["nutrients"];

                foreach (var item in nutrients)
                {
                    Nutrient nut = new Nutrient()
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
                            DisplayName = $"{meas["qty"].ToString()} {meas["label"].ToString()} ({meas["eqv"].ToString()} {nut.Unit})"
                        };
                        nut.Measures.Add(m);
                    }
                    f.Nutrients.Add(nut);
                }

                f.PortionIndex = targetIndex;
                f.SelectedPortionLabel = f.Nutrients[targetIndex-1].Measures[targetIndex-1].Label;
                f.SelectedPortionQty = f.Nutrients[targetIndex - 1].Measures[targetIndex - 1].Qty;
                f.NumberOfServings = 1;


            }

            return View(f);

        }
    }
}
