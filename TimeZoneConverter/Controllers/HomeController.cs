using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeZoneConverter.Models;

namespace TimeZoneConverter.Controllers
{
    public class HomeController : Controller
    {
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