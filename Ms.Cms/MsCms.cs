using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ms.Cms.Web;

namespace Ms.Cms
{
    public static class MsCms
    {
        public static HttpApplication Application { get; private set; }

        public static string LayoutView { get; private set; }

        public static string DesignerRole { get; private set; }

        public static void Setup(HttpApplication app, string layout = null, string designer = null)
        {
            Application = app;
            // TODO: provide default layout
            LayoutView = layout ?? "";
            DesignerRole = designer ?? "";

            // do web extraction
            WebExtractor.Extract(app);
        }
    }
}