using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FiTrack.Models;
using FiTrack.Models.LogicModels;

namespace FiTrack.Controllers
{
    public class LoggerController : Controller
    {

        /* how to reuse single client and dispose of when done?? */

/*
        private IHttpClientFactory _httpClientFactory;

        HttpClient client;


        public LoggerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            client = _httpClientFactory.CreateClient("UsdaAPI");

        }
*/

        public IActionResult Index()
        {
            return View();
        }


        public ActionResult SearchFoods(String foodName)
        {
            SearchHandler handler = new SearchHandler();

            var client = HttpClientAccessor.HttpClient;

            HttpResponseMessage response = client.GetAsync(handler.OrganizeSearchQ(foodName)).Result;

            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;

                return View(handler.StoreSearchReturns(dataObjects));
                
            }
            else
            {
                Debug.WriteLine("Something went wrong with the request I guess");
                return View();
            }

            
        }
  
    }
}