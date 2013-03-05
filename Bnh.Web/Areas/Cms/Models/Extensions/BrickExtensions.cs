using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Core;
using Cms.Helpers;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

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
        public static Brick GetContent(this Brick brick, IRepositories repos)
        {
            throw new NotImplementedException();
            //var list = repos.BrickContents.ToList();
            //var content = repos.BrickContents.FirstOrDefault(c => c.BrickContentId == brick.BrickContentId);
            //if (content != null)
            //{
            //    var linkableContent = content as LinkableBrick;
            //    if (linkableContent != null)
            //    {
            //        content = repos.BrickContents.FirstOrDefault(c => c.BrickContentId == linkableContent.LinkedContentId);
            //    }
            //}
            //return content ?? new Brick();
        }

        public static IDictionary<Brick, Brick> GetContentMap(this IEnumerable<Brick> bricks, IRepositories repos)
        {/*
            var brickList = bricks.Select(b => b.BrickContentId).ToList();

            var result = new Dictionary<Brick, Brick>();
            var contents = repos.BrickContents.Where(c => brickList.Contains(c.BrickContentId)).ToList();

            var linkable = contents.OfType<LinkableBrick>();
            var linkableList = linkable.Select(b => b.LinkedContentId).ToList();
            var linkableContents = repos.BrickContents.Where(c => linkableList.Contains(c.BrickContentId)).ToList();

            foreach (var b in bricks)
            {
                var content = contents.FirstOrDefault(c => b.BrickContentId == c.BrickContentId);
                if (content != null)
                {
                    var linkableContent = content as LinkableBrick;
                    if (linkableContent != null)
                    {
                        content = linkableContents.FirstOrDefault(c => c.BrickContentId == linkableContent.LinkedContentId);
                    }
                }
                result[b] = content ?? new EmptyContent();
            }
            return result;*/
            return null;
        }

        public static string GetSceneId(this Brick brick, IRepositories repos)
        {
            throw new NotImplementedException();
            //return repos.Scenes.Collection
            //    .Find(Query.EQ("Walls.Bricks.BrickId", ObjectId.Parse(brick.BrickId)))
            //    .Select(c => c.SceneId)
            //    .First();
        }
    }
}