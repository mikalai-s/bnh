using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Models
{
    public class TabsBrick : Brick
    {
        public string[] Tabs { get; set; }

        public string TabName { get; set; }
    }
}