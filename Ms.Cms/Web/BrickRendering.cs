using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Web;

using Newtonsoft.Json;

using Ms.Cms.Models;
using Ms.Cms.Models.Attributes;
using Ms.Cms.ViewModels;

namespace Ms.Cms
{
    public static class BrickRendering
    {
        private const string googleMapsKey = "AIzaSyBXgUOTPfgbS4kHE7fm_xr2za_O1ApA_TM";

        private static MvcHtmlString ToHtmlString(this string str)
        {
            return MvcHtmlString.Create(str);
        }
        /*
        private static string GetMapBrickContent(MapBrick mapBrick)
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
            return string.Format(html, mapBrick.GpsLocation, mapBrick.Height, mapBrick.Zoom ?? 10);
        }
        */
        public static string GetDiscriminant(this Brick brick)
        {
            return brick.GetType().Name;
        }

        
    }
}
