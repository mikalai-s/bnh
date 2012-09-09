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
        public static string GetSceneId(this BrickContent content, CmsEntities cms)
        {
            return cms.Scenes.Collection
                .Find(Query.EQ("Walls.Bricks.BrickContentId", ObjectId.Parse(content.BrickContentId)))
                .Select(c => c.SceneId)
                .First();
        }

        /// <summary>
        /// Gets brick title converted to HTML id string
        /// </summary>
        /// <param name="brick"></param>
        /// <returns></returns>
        public static string GetHtmlId(this BrickContent content)
        {
            return content.ContentTitle.ToHtmlId();
        }

        /// <summary>
        /// Returns view for current brick content
        /// </summary>
        /// <param name="brickContent"></param>
        /// <returns></returns>
        public static string GetBrickView(this BrickContent brickContent, HttpServerUtilityBase server)
        {
            if (brickContent == null)
            {
                return ContentUrl.Views.BrickContent.Partial.GetView(typeof(BrickContent));
            }

            var brickView = ContentUrl.Views.BrickContent.Partial.GetView(brickContent.GetType());
            return System.IO.File.Exists(server.MapPath(brickView))
                ? brickView
                : ContentUrl.Views.BrickContent.Partial.GetView(typeof(BrickContent));
        }

        /// <summary>
        /// Returns view for current brick content
        /// </summary>
        /// <param name="brickContent"></param>
        /// <returns></returns>
        public static string GetBrickEditView(this BrickContent brickContent, HttpServerUtilityBase server)
        {
            if (brickContent == null)
            {
                return ContentUrl.Views.BrickContent.Partial.GetEdit(typeof(BrickContent));
            }

            var brickView = ContentUrl.Views.BrickContent.Partial.GetEdit(brickContent.GetType());
            return System.IO.File.Exists(server.MapPath(brickView))
                ? brickView
                : ContentUrl.Views.BrickContent.Partial.GetView(typeof(BrickContent));
        }
    }
}