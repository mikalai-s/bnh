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
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Web;

namespace Bnh.WebFramework
{
    public static class BrickRendering
    {
        private const string googleMapsKey = "AIzaSyBXgUOTPfgbS4kHE7fm_xr2za_O1ApA_TM";

        public static MvcHtmlString BrickPrototype(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Brick(new Brick
            {
                Width = 100.0F,
            }, true);
        }

        public static MvcHtmlString Brick(this HtmlHelper htmlHelper, Brick brick, bool design = false)
        {
            var htmlFormat = design ?
@"<div class='brick-wrapper' style='width:{0}%' entity-data='{1}'>
    <div class='brick'>
        <div class='header'>
            <span class='title'>{2}</span>
            {3}
            <a class='delete'>delete</a>
        </div>
        <div class='content'>{4}</div>
        <div class='footer'></div>
    </div>
</div>"
:
@"<div class='brick-wrapper' style='width:{0}%'>
    {4}
</div>";


            var html = string.Format(htmlFormat,
                brick.Width.ToString("F"),
                brick.ToJson(),
                brick.Title,
                htmlHelper.ActionLink("edit", "Edit", "Brick", new { id = brick.Id }, new { @class = "edit" }),
                GetBrickContent(brick));

            return MvcHtmlString.Create(html);
        }

        private static object GetBrickContent(Brick brick)
        {
            var htmlBrick = brick as HtmlBrick;
            if (htmlBrick != null)
                return GetHtmlBrickContent(htmlBrick);
            var mapBrick = brick as MapBrick;
            if (mapBrick != null)
                return GetMapBrickContent(mapBrick);
            return string.Empty;
        }

        private static object GetMapBrickContent(MapBrick mapBrick)
        {
            // NOTE: in case of HTTPS change the url
            var html =
                @"<style type=""text/css"">
  html {{ height: 100% }}
  body {{ height: 100%; margin: 0; padding: 0 }}
  #map_canvas {{ height: 100% }}
</style>
<script type=""text/javascript""
      src=""http://maps.googleapis.com/maps/api/js?key=AIzaSyBXgUOTPfgbS4kHE7fm_xr2za_O1ApA_TM&sensor=false"">
</script>
<script type=""text/javascript"">
    $(function () {{
        var myLatlng = new google.maps.LatLng({0});
        var myOptions = {{
            center: myLatlng,
            zoom: {2},
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }};
        var map = new google.maps.Map(document.getElementById(""map_canvas""), myOptions);
        var marker = new google.maps.Marker({{
            position: myLatlng,
            map: map
        }});
    }});
</script>
<div class=""map-canvas-wrapper"" style=""height: {1}px"">
    <div id=""map_canvas""></div>
</div>
";
            return string.Format(html, mapBrick.GpsLocation, mapBrick.Height, mapBrick.Zoom);
        }

        private static object GetHtmlBrickContent(HtmlBrick htmlBrick)
        {
            return htmlBrick.Html;
        }

        public static string GetDiscriminant(this Brick brick)
        {
            return BnhModelBinder.HierarchyTypeMap[typeof(Brick)]
                .Where(v => v.Value == brick.GetType())
                .Select(e => e.Key)
                .First();
        }

        public static string ToJson(this Brick brick)
        {
            var properties = new Dictionary<string, object>();

            properties.Add("type", brick.GetDiscriminant());

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
