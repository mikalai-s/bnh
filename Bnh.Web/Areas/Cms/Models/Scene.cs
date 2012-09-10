using System;
using System.Collections.Generic;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Bnh.Cms.Models
{
    public partial class Scene
    {
        [BsonRepresentation(BsonType.ObjectId), BsonId]
        public string SceneId { get; set; }

        public string Title { get; set; }

        public bool IsTemplate { get; set; }

        public ICollection<Wall> Walls
        {
            get { return this._walls ?? (this._walls = new List<Wall>()); }
            set { this._walls = value; }
        } ICollection<Wall> _walls = null;
    }
}