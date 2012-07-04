using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms
{
    public static class ContentUrl
    {
        public static class Scripts
        {
            public const string EditHtmlBrick = "~/Ms.Cms/Scripts/EditHtmlBrick.js";

            public const string DesignScene = "~/Ms.Cms/Scripts/DesignScene.js";
        }

        public static class Styles
        {
            public const string DesignBrick = "~/Ms.Cms/Styles/DesignBrick.css";

            public const string DesignScene = "~/Ms.Cms/Styles/DesignScene.css";

            public const string Scene = "~/Ms.Cms/Styles/Scene.css";
        }

        public static class Views
        {
            public static class BrickContent
            {
                public const string Edit = "~/Ms.Cms/Views/BrickContent/Edit.cshtml";

                public const string View = "~/Ms.Cms/Views/BrickContent/View.cshtml";

                public static class Partial
                {
                    public static string GetEdit(Type type)
                    {
                        return "~/Ms.Cms/Views/BrickContent/Partial/Edit{0}.cshtml".FormatWith(type.Name);
                    }

                    public static string GetView(Type type)
                    {
                        return "~/Ms.Cms/Views/BrickContent/Partial/View{0}.cshtml".FormatWith(type.Name);
                    }
                }
            }

            public static class Scene
            {
                public const string Edit = "~/Ms.Cms/Views/Scene/Edit.cshtml";

                public const string View = "~/Ms.Cms/Views/Scene/View.cshtml";

                public static class Partial
                {
                    public const string DesignBrick = "~/Ms.Cms/Views/Scene/Partial/DesignBrick.cshtml";

                    public const string DesignScene = "~/Ms.Cms/Views/Scene/Partial/DesignScene.cshtml";

                    public const string DesignWall = "~/Ms.Cms/Views/Scene/Partial/DesignWall.cshtml";
                }
            }
        }
    }
}