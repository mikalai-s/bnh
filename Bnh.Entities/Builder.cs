using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bnh.Entities
{
    public class Builder
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        
        public string CityId { get; set; }

        public string UrlId { get; set; }
        
        public City City { get; set; }

        public IEnumerable<Community> Communities { get; set; }
    }
}
