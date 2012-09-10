using System;
using System.Collections.Generic;

namespace Bnh.Cms.Models
{
    public partial class MapContent : BrickContent
    {
        public string GpsLocation { get; set; }

        public int? Height { get; set; }

        public int? Zoom { get; set; }
    }
    
}
