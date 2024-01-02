using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TimeZoneConverter.Models;

namespace TimeZoneConverter.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        public ActionResult Index()
        {
            var model = new TimeZoneConverterModel
            {
                SelectedDateTime = DateTime.UtcNow,
                SelectedTimeZone = "UTC"
            };
            ViewBag.msg = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Convert(TimeZoneConverterModel model)
        {
            ViewBag.msg = "test";
            if (ModelState.IsValid)
            {
                TimeZoneInfo sourceTimeZone = TimeZoneInfo.Utc;
                TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(model.SelectedTimeZone);

                model.ConvertedDateTime = TimeZoneInfo.ConvertTime(model.SelectedDateTime, sourceTimeZone, targetTimeZone);
            }

            return View("Index", model);
        }
        [HttpPost]
        public async Task<ActionResult> Convert2(TimeZoneConverterModel model)
        {
            ViewBag.msg = "test";

            string apiUrl = "https://timeapi.io/api/Conversion/ConvertTimeZone";
            using (HttpClient client = new HttpClient())
            {
                // Prepare the request payload
                var requestPayload = new
                {
                    fromTimeZone = "Europe/Amsterdam",
                    dateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:MM:ss"),
                    toTimeZone = "America/Los_Angeles",
                    dstAmbiguity = ""
                };

                // Convert the payload to a JSON string
                string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(requestPayload);

                // Create the HTTP content with the JSON payload
                HttpContent content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    // Make the POST request
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Check if the request was successful (status code 200-299)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and print the response content
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response: " + responseBody);
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }

            return View("Index", model);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}