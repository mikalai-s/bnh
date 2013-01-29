using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Cms.Repositories;

namespace Bnh.Cms.Models
{
    public static class BrickExtensions
    {
        /// <summary>
        /// Gets brick content.
        /// NOTE: ensures linked content if content is linkable.
        /// </summary>
        /// <param name="brick"></param>
        /// <returns></returns>
        public static BrickContent GetContent(this Brick brick, CmsRepos db)
        {
            var list = db.BrickContents.ToList();
            var content = db.BrickContents.FirstOrDefault(c => c.BrickContentId == brick.BrickContentId);
            if (content != null)
            {
                var linkableContent = content as LinkableContent;
                if (linkableContent != null)
                {
                    content = db.BrickContents.FirstOrDefault(c => c.BrickContentId == linkableContent.LinkedContentId);
                }
            }
            return content ?? new EmptyContent();
        }
    }
}