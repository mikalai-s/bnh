using System;
using System.Collections.Generic;

namespace Cms.Models
{
    public partial class MapBrick : Brick
    {
        public string GpsLocation { get; set; }

        public int? Height { get; set; }

        public int? Zoom { get; set; }

        // TODO: add bool property to determine on brick view what to show 
        // - mappble is interface is implemeted or something custom
    }
}
