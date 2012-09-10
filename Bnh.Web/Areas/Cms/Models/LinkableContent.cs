using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bnh.Cms.Models
{
    public partial class LinkableContent : BrickContent
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string LinkedContentId { get; set; }
    }
    
}
