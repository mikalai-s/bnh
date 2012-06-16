
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System;
using System.Reflection;
using System.Text;

using Ms.Cms.Models;
using System.IO;

namespace Ms.Cms
{
    // Summary:
    //     Represents support for HTML links in an application.
    public static class HtmlExtensions
    {
        private static readonly Dictionary<Type, string> BrickTypeNames = new Dictionary<Type, string>()
                                                                     {
                                                                         {typeof(HtmlBrick), "Rich Text"},
                                                                         {typeof(RazorBrick), "Razor Template"},
                                                                         {typeof(GalleryBrick), "Gallery"},
                                                                         {typeof(MapBrick), "Map"},
                                                                         {typeof(EmptyBrick), "Empty"},
                                                                         {typeof(LinkableBrick), "Linkable"},
                                                                         {typeof(TocBrick), "Table of Content"}
                                                                     };

        public static string GetBrickTypeName(Type brickType)
        {
            return BrickTypeNames[brickType];
        }

        public static MvcHtmlString DropDownListForBrickTypes(this HtmlHelper htmlHelper, string name)
        {
            var items = BrickTypeNames
                .Select(e => new SelectListItem { Value = e.Key.Name, Text = e.Value });
            return htmlHelper.DropDownList(name, items);
        }

        public static MvcHtmlString EditSceneLink(this HtmlHelper html, Scene scene, string text)
        {
            return html.ActionLink(text, "Edit", "Scene", new { id = scene.Id }, null);
        }


        public static MvcHtmlString Scene(this HtmlHelper html, Scene scene)
        {
            return html.Partial("~/WebExtracted/Ms.Cms/Views/Scene/Scene.cshtml", scene);
        }

        // adopted from http://stackoverflow.com/questions/483091/render-a-view-as-a-string
        private static MvcHtmlString RenderView<T>(this HtmlHelper helper, string viewName, T model)
        {
            helper.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(helper.ViewContext.Controller.ControllerContext, viewName);
                var viewContext = new ViewContext(helper.ViewContext.Controller.ControllerContext, viewResult.View, helper.ViewData, helper.ViewContext.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(helper.ViewContext.Controller.ControllerContext, viewResult.View);
                return MvcHtmlString.Create(sw.GetStringBuilder().ToString());
            }
        }
    }
}