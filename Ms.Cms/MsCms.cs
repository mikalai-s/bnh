using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ms.Cms.Models;
using Ms.Cms.Models.Utilities;

namespace Ms.Cms
{
    public static class MsCms
    {
        public static HttpApplication Application { get; private set; }

        public static string LayoutView { get; private set; }

        public static string DesignerRole { get; private set; }

        public static string TinyMceScript { get; private set; }

        public static IEnumerable<BrickRegistration> RegisteredBrickTypes { get; private set; }

        public static void Setup(HttpApplication app, string layout = null, string designer = null, string tinymce = null, string googleMaps = null, IEnumerable<BrickRegistration> bricks = null)
        {
            Application = app;
            // TODO: provide default layout
            LayoutView = layout ?? "";
            DesignerRole = designer ?? "";
            TinyMceScript = tinymce ?? "";
            GoogleMapsApiScript = googleMaps ?? "";
            RegisteredBrickTypes = GetRegisteredBrickTypes(bricks ?? Enumerable.Empty<BrickRegistration>()).ToList();

            // do web extraction
            WebExtractor.Extract(app);
        }

        private static IEnumerable<BrickRegistration> GetRegisteredBrickTypes(IEnumerable<BrickRegistration> externalBricks)
        {
            yield return new BrickRegistration { Type = typeof(HtmlBrick), Title = "Rich Text", View = "" };
            yield return new BrickRegistration { Type = typeof(RazorBrick), Title = "Razor Template", View = "" };
            yield return new BrickRegistration { Type = typeof(MapBrick), Title = "Map", View = "" };
            yield return new BrickRegistration { Type = typeof(EmptyBrick), Title = "Empty", View = "" };
            yield return new BrickRegistration { Type = typeof(LinkableBrick), Title = "Linkable", View = "" };
            yield return new BrickRegistration { Type = typeof(TocBrick), Title = "Table of Content", View = "" };

            foreach (var externalBrick in externalBricks)
            {
                yield return externalBrick;
            }
        }

        public static string GoogleMapsApiScript { get; set; }
    }
}