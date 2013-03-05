using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cms.Models
{
    public class SpecialScene : ISceneHolder
    {
        [BsonRepresentation(BsonType.ObjectId), BsonId]
        public string SceneId { get; set; }

        public string Title { get; set; }

        public Scene Scene { get; set; }
    }
}