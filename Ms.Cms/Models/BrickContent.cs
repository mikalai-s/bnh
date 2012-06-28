using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Builders;

namespace Ms.Cms.Models
{
    public abstract class BrickContent
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ContentTitle { get; set; }

        public string GetSceneId()
        {
            using (var cms = new CmsEntities())
            {
                return cms.Scenes.Collection.Find(Query.EQ("Walls.Bricks.BrickContentId", ObjectId.Parse(this.Id))).First().Id;
            }
        }
    }
}