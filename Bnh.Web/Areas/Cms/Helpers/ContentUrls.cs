using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Helpers
{
    public static class ContentUrl
    {
        public static class Scripts
        {
            public const string EditHtmlBrick = "~/Areas/Cms/Scripts/edit-html-brick.js";

            public const string DesignScene = "~/Areas/Cms/Scripts/design-scene.js";
        }

        public static class Styles
        {
            public const string DesignBrick = "~/Areas/Cms/Styles/DesignBrick.css";

            public const string DesignScene = "~/Areas/Cms/Styles/DesignScene.css";

            public const string Scene = "~/Areas/Cms/Styles/Scene.css";
        }

        public static class Views
        {
            public static class BrickContent
            {
                public const string Edit = "~/Areas/Cms/Views/BrickContent/Edit.cshtml";

                public const string View = "~/Areas/Cms/Views/BrickContent/View.cshtml";

                public static class Partial
                {
                    public static string GetEdit(Type type)
                    {
                        return "~/Areas/Cms/Views/BrickContent/Partial/Edit{0}.cshtml".FormatWith(type.Name);
                    }

                    public static string GetView(Type type)
                    {
                        return "~/Areas/Cms/Views/BrickContent/Partial/View{0}.cshtml".FormatWith(type.Name);
                    }
                }
            }

            public static class Scene
            {
                public const string Edit = "~/Areas/Cms/Views/Scene/Edit.cshtml";

                public const string EditLinkable = "~/Areas/Cms/Views/Scene/EditLinkable.cshtml";

                public const string View = "~/Areas/Cms/Views/Scene/View.cshtml";

                public static class Partial
                {
                    public const string DesignBrick = "~/Areas/Cms/Views/Scene/Partial/DesignBrick.cshtml";

                    public const string DesignScene = "~/Areas/Cms/Views/Scene/Partial/DesignScene.cshtml";

                    public const string DesignWall = "~/Areas/Cms/Views/Scene/Partial/DesignWall.cshtml";
                }
            }
        }
    }
}