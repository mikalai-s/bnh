using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Cms.Models
{
    public class GalleryContent : BrickContent
    {
        public int? Height { get; set; }

        public string[] Images { get; set; }
    }
}