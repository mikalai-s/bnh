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

        public static IDictionary<Brick, BrickContent> GetContentMap(this IEnumerable<Brick> bricks, IRepositories repos)
        {
            var brickList = bricks.Select(b => b.BrickContentId).ToList();

            var result = new Dictionary<Brick, BrickContent>();
            var contents = repos.BrickContents.Where(c => brickList.Contains(c.BrickContentId)).ToList();

            var linkable = contents.OfType<LinkableContent>();
            var linkableList = linkable.Select(b => b.LinkedContentId).ToList();
            var linkableContents = repos.BrickContents.Where(c => linkableList.Contains(c.BrickContentId)).ToList();

            foreach (var b in bricks)
            {
                var content = contents.FirstOrDefault(c => b.BrickContentId == c.BrickContentId);
                if (content != null)
                {
                    var linkableContent = content as LinkableContent;
                    if (linkableContent != null)
                    {
                        content = linkableContents.FirstOrDefault(c => c.BrickContentId == linkableContent.LinkedContentId);
                    }
                }
                result[b] = content ?? new EmptyContent();
            }
            return result;
        }
    }
}