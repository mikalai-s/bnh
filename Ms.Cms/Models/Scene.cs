using System;
using System.Collections.Generic;

using Ms.Cms.Models.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ms.Cms.Models
{
    public partial class Scene
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public bool IsTemplate { get; set; }

        // TODO: remove this thing when we get rid of SQL completely
        public Guid OwnerGuidId { get; set; }

        [NonJsExposable]
        public ICollection<Wall> Walls
        {
            get { return this._walls ?? (this._walls = new List<Wall>()); }
            set { this._walls = value; }
        } ICollection<Wall> _walls = null;
    }
}