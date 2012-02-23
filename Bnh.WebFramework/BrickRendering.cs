using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Bnh.Entities;

namespace Bnh.WebFramework
{
    public static class BrickRendering
    {
        public static MvcHtmlString Brick(this HtmlHelper htmlHelper, Brick brick)
        {
            var brickDiv = new TagBuilder("div");
            brickDiv.AddCssClass("brick");

            var htmlBrick = brick as HtmlBrick;
            if (htmlBrick != null)
            {
                brickDiv.InnerHtml = htmlBrick.Html;
            }

            

            var wrapperDiv = new TagBuilder("div");
            wrapperDiv.AddCssClass("brick-wrapper");
            wrapperDiv.Attributes["style"] = "width: " + brick.Width.ToString("F") + "%";
            wrapperDiv.InnerHtml = brickDiv.ToString();

            return MvcHtmlString.Create(wrapperDiv.ToString());
        }
    }
}
