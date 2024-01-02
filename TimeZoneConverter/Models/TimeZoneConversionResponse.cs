using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeZoneConverter.Models
{
    public class TimeZoneConversionResponse
    {
        public long fromTimestamp { get; set; }
        public long toTimestamp { get; set; }
    }
}