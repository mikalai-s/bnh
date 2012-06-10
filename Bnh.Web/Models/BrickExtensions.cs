using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ms.Cms.Models;

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

        public static Brick EnsureNonLinked(this Brick brick)
        {
            var linkable = brick as LinkableBrick;
            return (linkable == null) ? brick : linkable.LinkedBrick;
        }
    }
}