using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Core;

namespace Cms.Models
{
    public static class BrickExtensions
    {
        /// <summary>
        /// Gets brick content.
        /// NOTE: ensures linked content if content is linkable.
        /// </summary>
        /// <param name="brick"></param>
        /// <returns></returns>
        public static BrickContent GetContent(this Brick brick, IRepositories repos)
        {
            var list = repos.BrickContents.ToList();
            var content = repos.BrickContents.FirstOrDefault(c => c.BrickContentId == brick.BrickContentId);
            if (content != null)
            {
                var linkableContent = content as LinkableContent;
                if (linkableContent != null)
                {
                    content = repos.BrickContents.FirstOrDefault(c => c.BrickContentId == linkableContent.LinkedContentId);
                }
            }
            return content ?? new EmptyContent();
        }
    }
}