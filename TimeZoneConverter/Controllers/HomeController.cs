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
        public async Task<ActionResult> Convert2Async(TimeZoneConverterModel model)
        {
            ViewBag.msg = "test";

            string apiKey = "<your_api_key>";

            // Replace with your source and target timezones
            string source = "America/New_York";
            string target = "Europe/London";

           
            long utcTimestamp =(long)DateTime.UtcNow.Second;

            // Make API request to TimeZoneDb
            string apiUrl = $"http://api.timezonedb.com/v2.1/convert-time-zone?key={apiKey}&from={source}&to={target}&time={utcTimestamp}";

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    // Parse the response
                    TimeZoneConversionResponse conversionResponse = JsonConvert.DeserializeObject<TimeZoneConversionResponse>(content);
                    model.ConvertedDateTime = UnixTimeStampToDateTime(conversionResponse.toTimestamp);

                    //Console.WriteLine($"UTC Time: {UnixTimeStampToDateTime(utcTimestamp)}");
                    //Console.WriteLine($"Source Time ({source}): {UnixTimeStampToDateTime(conversionResponse.fromTimestamp)}");
                    //Console.WriteLine($"Target Time ({target}): {UnixTimeStampToDateTime(conversionResponse.toTimestamp)}");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return View("Index", model);
        }
        private static long GetCurrentUnixTimestamp()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp);
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