using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Models
{
    public class GalleryBrick : Brick
    {
        public int? Height { get; set; }

        public string[] Images { get; set; }
    }
}