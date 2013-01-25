using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Builders;

namespace Bnh.Cms.Models
{
    public abstract class BrickContent
    {
        [BsonRepresentation(BsonType.ObjectId), BsonId]
        public string BrickContentId { get; set; }

        public string ContentTitle { get; set; }

        public bool IsTitleUsedInToC { get; set; }

        public bool IsTitleVisible { get; set; }
    }
}