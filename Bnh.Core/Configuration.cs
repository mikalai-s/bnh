using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Core
{
    public class Configuration
    {
        public IDictionary<string, string> ConnectionStrings;
        public string City { get; set; }
        public ReviewConfiguration Review { get; set; }
    }

    public class ReviewConfiguration
    {
        public string[] Questions { get; set; }
    }
}