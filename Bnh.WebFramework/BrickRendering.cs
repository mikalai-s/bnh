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
        public static MvcHtmlString BrickPrototype(this HtmlHelper htmlHelper, bool design = false)
        {
            return htmlHelper.Brick(new Brick
            {
                Width = 100.0F,
            }, design);
        }

        public static MvcHtmlString Brick(this HtmlHelper htmlHelper, Brick brick, bool design = false)
        {
            var htmlFormat = design ?
@"<div class='brick-wrapper' style='width:{0}%' entity-data='{1}'>
    <div class='brick'>
        <div class='header'>
            <span class='title'>{2}</span>
            <a class='edit'>edit</a>|
            <a class='delete'>delete</a>
        </div>
        <div class='content'></div>
        <div class='footer'></div>
    </div>
</div>"
:
@"<div class='brick-wrapper' style='width:{0}%' entity-data='{1}'>
    <div class='brick'>
        <div class='header'>
            <span class='title'>{2}</span>
        </div>
        <div class='content'></div>
        <div class='footer'></div>
    </div>
</div>";


            var html = string.Format(htmlFormat, 
                brick.Width.ToString("F"), 
                brick.ToJson(),
                brick.Title);

            return MvcHtmlString.Create(html);
        }

        public static string ToJson(this Brick brick)
        {
            var properties = new Dictionary<string, object>();

            properties.Add("type", BnhModelBinder.HierarchyTypeMap[typeof(Brick)]
                .Where(v => v.Value == brick.GetType()).Select(e => e.Key).First());

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
                    properties.Add(prop.Name.ToLower(), value);
            }
            return new JavaScriptSerializer().Serialize(properties);
        }
    }
}
