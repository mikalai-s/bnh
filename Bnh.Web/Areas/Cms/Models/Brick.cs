using System;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.Serialization;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cms.Models
{
    public partial class Brick
    {
        public string Title { get; set; }

        public float Width { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string BrickContentId { get; set; }


        #region UI Specific Properties
        // TODO: use viewmodel instead

        [BsonIgnore]
        public string NewContentTypeName { get; set; }

        #endregion
    }
}
