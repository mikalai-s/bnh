using System;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.Serialization;

using Ms.Cms.Models.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ms.Cms.Models
{
    public partial class Brick
    {
        public string Title { get; set; }

        public float Width { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string BrickContentId { get; set; }
    }
}
