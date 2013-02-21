using System;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.Serialization;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cms.Models
{
    public class Brick
    {
        [BsonRepresentation(BsonType.ObjectId), BsonId]
        public string BrickId { get; set; }

        public string Title { get; set; }

        public float Width { get; set; }

        public bool IsTitleUsedInToC { get; set; }

        public bool IsTitleVisible { get; set; }
    }
}
