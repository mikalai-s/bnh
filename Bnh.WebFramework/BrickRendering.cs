using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Bnh.Entities;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.ComponentModel;

namespace Bnh.WebFramework
{
    public static class BrickRendering
    {
        public static MvcHtmlString BrickPrototype(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Brick(new Brick
            {
                Width = 100.0F,
            });
        }

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
            wrapperDiv.Attributes["entity"] = brick.ToJson();
            wrapperDiv.InnerHtml = brickDiv.ToString();

            return MvcHtmlString.Create(wrapperDiv.ToString());
        }

        public static string ToJson(this Brick brick)
        {
            var properties = new Dictionary<string, string>();
            foreach (var prop in brick.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                // NOTE: for HtmlBrick we are not serializing Html property - it's too big
                if (brick is HtmlBrick && prop.Name == "Html")
                    continue;

                var p = prop.GetCustomAttributes(typeof(DataMemberAttribute), false);
                if (p.Length == 0)
                    continue;

                p = prop.GetCustomAttributes(typeof(XmlIgnoreAttribute), false);
                if(p.Length > 0)
                    continue;

                p = prop.GetCustomAttributes(typeof(SoapIgnoreAttribute), false);
                if(p.Length > 0)
                    continue;

                p = prop.GetCustomAttributes(typeof(BrowsableAttribute), false);
                if (p.Length > 0 && !(p[0] as BrowsableAttribute).Browsable)
                    continue;

                var value = prop.GetValue(brick, null);
                if(value != null)
                    properties.Add(prop.Name, value.ToString());
            }
            return new JavaScriptSerializer().Serialize(properties);
        }
    }
}
