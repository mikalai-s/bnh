using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms.Models
{
    public static class BrickExtensions
    {
        /// <summary>
        /// Gets brick title converted to HTML id string
        /// </summary>
        /// <param name="brick"></param>
        /// <returns></returns>
        public static string GetHtmlId(this Brick brick)
        {
            return "";//brick.Title.ToHtmlId();
        }

        /// <summary>
        /// Gets brick content.
        /// NOTE: ensures linked content if content is linkable.
        /// </summary>
        /// <param name="brick"></param>
        /// <returns></returns>
        public static BrickContent GetContent(this Brick brick)
        {
            using(var db = new CmsEntities())
            {
                var list = db.BrickContents.ToList();
                var content = db.BrickContents.FirstOrDefault(c => c.Id == brick.BrickContentId);
                if (content != null)
                {
                    var linkableContent = content as LinkableContent;
                    if (linkableContent != null)
                    {
                        content = db.BrickContents.FirstOrDefault(c => c.Id == linkableContent.LinkedContentId);
                    }
                }
                return content ?? new EmptyContent();
            }
        }
    }
}