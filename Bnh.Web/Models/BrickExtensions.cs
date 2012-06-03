using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ms.Cms.Entities;

namespace Bnh.Web.Models
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
            return brick.Title.ToHtmlId();
        }

        public static Brick EnsureNonShared(this Brick brick)
        {
            var shared = brick as SharedBrick;
            return (shared == null) ? brick : shared.Share;
        }
    }
}