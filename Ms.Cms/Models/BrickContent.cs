using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ms.Cms.Models
{
    public abstract class BrickContent
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ContentTitle { get; set; }
    }
}