using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bnh.Entities
{
    public class City
    {
        [BsonRepresentation(BsonType.ObjectId), BsonId]
        public string CityId { get; set; }
        
        public string Name { get; set; }

        public string UrlId { get; set; }

        public IEnumerable<string> Zones { get; set; }

        public IEnumerable<Community> Communities { get; set; }

        // tODo: do rlatino between communities and cities
    }
}
