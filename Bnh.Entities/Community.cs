using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bnh.Entities
{
    public class Community
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [ExternalProperty(Title = "Community")]
        public string Name { get; set; }

        public string CityId { get; set; }
        
        public string Zone { get; set; }
        
        public string UrlId { get; set; }

        [ExternalProperty]
        public string Developer { get; set; }

        [ExternalProperty]
        public int Established { get; set; }

        [ExternalProperty]
        public string Coverage { get; set; }

        [ExternalProperty]
        public string Ward { get; set; }
        
        public string GpsLocation { get; set; }
        
        public string GpsBounds { get; set; }

        [ExternalProperty]
        public string CurrentPhase { get; set; }
        
        [FilterProperty(Title = "To city center", Operator = FilterOperator.LessOrEqual)]
        public int Remoteness { get; set; }
        
        [FilterProperty(Title = "Has Lake")]
        public bool HasLake { get; set; }
        
        [FilterProperty(Title = "Has Water Feature")]
        public bool HasWaterFeature { get; set; }
        
        [FilterProperty(Title = "Has Club Or Facility")]
        public bool HasClubOrFacility { get; set; }
        
        [FilterProperty(Title = "Has Mountain View")]
        public bool HasMountainView { get; set; }
        
        [FilterProperty(Title = "Has Parks And Pathways")]
        public bool HasParksAndPathways { get; set; }
        
        [FilterProperty(Title = "Has Shopping Plaza")]
        public bool HasShoppingPlaza { get; set; }
        
        [ExternalProperty(Title = "Number of Playgrounds")]
        public int NumberOfPlaygrounds { get; set; }
        
        [ExternalProperty(Title = "Number of Dwellings")]
        public int NumberOfDwellings { get; set; }
        
        [ExternalProperty]
        public int Population { get; set; }
        
        [ExternalProperty(Title = "Transit Bus")]
        public string TransitBus { get; set; }
        
        [ExternalProperty]
        public string Schools { get; set; }        
        
        public IEnumerable<Builder> Builders { get; set; }

        public string SceneId { get; set; }
    }
}
