using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeZoneConverter.Models
{
    public class TimeZoneConverterModel
    {
        public DateTime SelectedDateTime { get; set; }
        public string SelectedTimeZone { get; set; }
        public DateTime ConvertedDateTime { get; set; }
    }
}