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
                // TODO: provide corresponding index to speed up the query
                return cms.Scenes.Collection
                    .Find(Query.EQ("Walls.Bricks.BrickContentId", ObjectId.Parse(content.Id)))
                    .Select(c => c.Id)
                    .First();
            }
        }       
    }
}