using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cms.Models
{
    public partial class LinkableBrick : Brick
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string LinkedBrickId { get; set; }
    }
    
}
