﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Core
{
    public class Config
    {
        public IDictionary<string, string> ConnectionStrings;
        public string City { get; set; }
        public ReviewConfig Review { get; set; }
    }

    public class ReviewConfig
    {
        public IDictionary<string, string> Questions { get; set; }
    }
}