using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bnh.Entities
{
    public class Review
    {
        [BsonRepresentation(BsonType.ObjectId), BsonId]
        public string ReviewId { get; set; }

        public string Message { get; set; }

        public int[] Ratings { get; set; }
    }
}
