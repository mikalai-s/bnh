using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ms.Cms.Models
{
    public class DesignBrick : Brick
    {
        public string TypeName { get; set; }

        public int? OriginalWallIndex { get; set; }

        public int? OriginalIndex { get; set; }
    }
}