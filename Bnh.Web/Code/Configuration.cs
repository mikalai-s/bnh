using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Web.Code
{
    public class Configuration
    {
        public ReviewConfiguration Review { get; set; }
    }

    public class ReviewConfiguration
    {
        public string[] Questions { get; set; }
    }
}