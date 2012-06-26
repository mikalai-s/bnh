using System;
using System.Collections.Generic;

namespace Ms.Cms.Models
{
    public partial class MapContent : BrickContent
    {
        public string GpsLocation { get; set; }

        public int? Height { get; set; }

        public int? Zoom { get; set; }
    }
    
}
