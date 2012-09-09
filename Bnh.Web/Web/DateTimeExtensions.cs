using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Bnh.Web
{
    public static class DateTimeExtensions
    {
        public static string ToUserFriendlyString(this DateTime dt)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            return 
                dt.ToString("MMMM dd, yyyy", culture.DateTimeFormat) +
                " at " + 
                dt.ToString("h:mm:ss tt", culture.DateTimeFormat);
        }
    }
}