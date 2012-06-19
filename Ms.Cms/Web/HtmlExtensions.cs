
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
using System.Web.WebPages;
using System.Web.Optimization;

namespace Ms.Cms
{
    // Summary:
    //     Represents support for HTML links in an application.
    public static class HtmlExtensions
    {
        public static MvcHtmlString DropDownListForBrickTypes(this HtmlHelper htmlHelper, string name)
        {
            var items = MsCms.RegisteredBrickTypes
                .Select(br => new SelectListItem { Value = br.Type.Name, Text = br.Title });
            return htmlHelper.DropDownList(name, items);
        }

        public static MvcHtmlString EditSceneLink(this HtmlHelper html, Scene scene, string text)
        {
            return html.ActionLink(text, "Edit", "Scene", new { id = scene.Id }, null);
        }

        public static List<string> GetStyleBundle(this WebViewPage page)
        {
            var bundle = page.ViewContext.Controller.ViewBag._MsCms_StyleBundle as List<string>;
            if (bundle == null)
            {
                page.ViewContext.Controller.ViewBag._MsCms_StyleBundle = bundle = new List<string>();
            }
            return bundle;
        }

        public static List<string> GetScriptBundle(this WebViewPage page)
        {
            var scriptBundle = page.ViewContext.Controller.ViewBag._MsCms_ScriptBundle as List<string>;
            if (scriptBundle == null)
            {
                page.ViewContext.Controller.ViewBag._MsCms_ScriptBundle = scriptBundle = new List<string>();
            }
            return scriptBundle;
            //var scriptBundle = page.ViewBag._MsCms_ScriptBundle as ScriptBundle;
            //if (scriptBundle == null)
            //{
            //    page.ViewBag._MsCms_ScriptBundle = scriptBundle = new ScriptBundle("~/WebExtracted/Ms.Cms/Scripts/js");
            //}
            //return scriptBundle;
        }


        public static MvcHtmlString RenderScene(this WebViewPage page, Scene scene)
        {
            var s =  page.Html.Partial("~/WebExtracted/Ms.Cms/Views/Scene/Scene.cshtml", scene);
            page.RenderStylesAndScripts();
            return s;
        }

        public static void RenderStylesAndScripts(this WebViewPage page)
        {
            // define styles
            page.DefineSection("_MsCmsStyles", () =>
            {
                page.Write(Styles.Render(page.GetStyleBundle().Distinct().ToArray()));
            });

            // define scripts
            page.DefineSection("_MsCmsScripts", () =>
            {
                page.Write(Scripts.Render(page.GetScriptBundle().Distinct().ToArray()));
            });
        }

        /// <summary>
        /// NOTE: just a temprory solution to be compatible with ScriptBundle.Include() method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        public static void Include<T>(this List<T> list, IEnumerable<T> items)
        {
            list.AddRange(items);
        }
    }
}