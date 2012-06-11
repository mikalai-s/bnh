using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ms.Cms.Models
{
    public partial class MapBrick : Brick
    {
        [MaxLength(100)]
        public string GpsLocation { get; set; }

        public int? Height { get; set; }

        public int? Zoom { get; set; }
    }
    
}
