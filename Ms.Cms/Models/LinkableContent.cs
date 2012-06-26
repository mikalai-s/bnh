using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Ms.Cms.Models.Attributes;

namespace Ms.Cms.Models
{
    public partial class LinkableContent : BrickContent
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string LinkedContentId { get; set; }

        //[NonJsExposable]
        //public Brick LinkedBrick { get; set; }
    }
    
}
