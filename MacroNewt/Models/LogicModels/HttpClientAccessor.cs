using System;
using System.Net.Http;

namespace MacroNewt.Models.LogicModels
{
    /*
     *  The HttpClientAccessor class
     *  Used any time an API call is made
     */

    /// <summary>
    /// This class is used for any USDA API calls
    /// </summary>
    /// <remarks>
    /// Any USDA API request uses this class to establish an <see cref="HttpClient"/> object with appropriate Uri and headers
    /// </remarks>
    public static class HttpClientAccessor
    {
        /// <summary>
        /// Establishes HttpClient with the correct Uri and headers
        /// </summary>
        public static Func<HttpClient> UsdaFactory = () =>
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri("https://api.nal.usda.gov/ndb/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        };

        private static Lazy<HttpClient> client = new Lazy<HttpClient>(UsdaFactory);

        /// <summary>
        /// Provides the HttpClient's lazy value 
        /// </summary>
        public static HttpClient HttpClient
        {
            get { return client.Value; }
        }

    }
}
