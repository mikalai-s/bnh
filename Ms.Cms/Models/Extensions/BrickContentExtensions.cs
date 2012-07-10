using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace Ms.Cms.Models
{
    public static class BrickContentExtensions
    {
        public static string GetSceneId(this BrickContent content)
        {
            using (var cms = new CmsEntities())
            {
                return cms.Scenes.Collection
                    .Find(Query.EQ("Walls.Bricks.BrickContentId", ObjectId.Parse(content.BrickContentId)))
                    .Select(c => c.SceneId)
                    .First();
            }
        }       
    }
}