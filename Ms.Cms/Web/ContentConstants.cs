using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms.Web
{
    internal static class ContentConstants
    {
        public static class Views
        {
            public static class BrickContent
            {
                public const string Edit = "/Views/BrickContent/Edit.cshtml";

                public const string View = "/Views/BrickContent/View.cshtml";

                public static class Partial
                {
                    public const string EditHtmlContent = "/Views/BrickContent/Partial/EditHtmlContent.cshtml";

                    public const string EditLinkableContent = "/Views/BrickContent/Partial/EditLinkableContent.cshtml";

                    public const string EditMapContent = "/Views/BrickContent/Partial/EditMapContent.cshtml";

                    public const string EditRazorContent = "/Views/BrickContent/Partial/EditRazorContent.cshtml";

                    public const string ViewHtmlContent = "/Views/BrickContent/Partial/ViewHtmlContent.cshtml";

                    public const string ViewLinkableContent = "/Views/BrickContent/Partial/ViewLinkableContent.cshtml";

                    public const string ViewMapContent = "/Views/BrickContent/Partial/ViewMapContent.cshtml";

                    public const string ViewRazorContent = "/Views/BrickContent/Partial/ViewRazorContent.cshtml";
                }
            }

            public static class Scene
            {
                public const string Edit = "/Views/Scene/Edit.cshtml";

                public const string View = "/Views/Scene/View.cshtml";

                public static class Partial
                {
                    public const string DesignBrick = "/Views/Scene/Partial/DesignBrick.cshtml";

                    public const string DesignScene = "/Views/Scene/Partial/DesignScene.cshtml";

                    public const string DesignWall = "/Views/Scene/Partial/DesignWall.cshtml";
                }
            }
        }
    }
}