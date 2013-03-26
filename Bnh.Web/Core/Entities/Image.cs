using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Core.Entities
{
    public class Image
    {
        public string Url { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }
    }
}