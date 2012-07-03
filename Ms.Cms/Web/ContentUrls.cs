using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms
{
    public static class ContentUrl
    {
        public static string Resolve(string path)
        {
            return "~" + MsCms.WebExtractorUrl + "/Ms.Cms" + path;
        }

        public static class Scripts
        {
            public static string EditHtmlBrick { get { return Resolve("/Scripts/EditHtmlBrick.js"); } }
        }

        public static class Styles
        {
            public static string DesignBrick { get { return Resolve("/Styles/DesignBrick.css"); } }
        }

        public static class Views
        {
            public static class BrickContent
            {
                public static string Edit { get { return Resolve("/Views/BrickContent/Edit.cshtml"); } }

                public static string View { get { return Resolve("/Views/BrickContent/View.cshtml"); } }

                public static class Partial
                {
                    public static string EditHtmlContent { get { return Resolve("/Views/BrickContent/Partial/EditHtmlContent.cshtml"); } }

                    public static string EditLinkableContent { get { return Resolve("/Views/BrickContent/Partial/EditLinkableContent.cshtml"); } }

                    public static string EditMapContent { get { return Resolve("/Views/BrickContent/Partial/EditMapContent.cshtml"); } }

                    public static string EditRazorContent { get { return Resolve("/Views/BrickContent/Partial/EditRazorContent.cshtml"); } }

                    public static string ViewHtmlContent { get { return Resolve("/Views/BrickContent/Partial/ViewHtmlContent.cshtml"); } }

                    public static string ViewLinkableContent { get { return Resolve("/Views/BrickContent/Partial/ViewLinkableContent.cshtml"); } }

                    public static string ViewMapContent { get { return Resolve("/Views/BrickContent/Partial/ViewMapContent.cshtml"); } }

                    public static string ViewRazorContent { get { return Resolve("/Views/BrickContent/Partial/ViewRazorContent.cshtml"); } }
                }
            }

            public static class Scene
            {
                public static string Edit { get { return Resolve("/Views/Scene/Edit.cshtml"); } }

                public static string View { get { return Resolve("/Views/Scene/View.cshtml"); } }

                public static class Partial
                {
                    public static string DesignBrick { get { return Resolve("/Views/Scene/Partial/DesignBrick.cshtml"); } }

                    public static string DesignScene { get { return Resolve("/Views/Scene/Partial/DesignScene.cshtml"); } }

                    public static string DesignWall { get { return Resolve("/Views/Scene/Partial/DesignWall.cshtml"); } }
                }
            }
        }
    }
}