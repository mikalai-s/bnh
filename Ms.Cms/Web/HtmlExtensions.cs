
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
using Ms.Cms.ViewModels;

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

        public static MvcHtmlString EditSceneLink(this HtmlHelper html, Scene scene, string text, Uri viewUrl)
        {
            return html.ActionLink(text, "Edit", "Scene", new { id = scene.SceneId, returnUrl = viewUrl.AbsoluteUri }, null);
        }

        public static string GetBackUrl(this RequestContext rc)
        {
            return rc.HttpContext.Session["_lastUsedSceneDesignUrl"] as string ?? 
                   rc.HttpContext.Request.UrlReferrer + "";
        }

        public static ViewContext GetTopViewContext(ViewContext viewContext)
        {
            if (viewContext.ParentActionViewContext != null)
                return GetTopViewContext(viewContext.ParentActionViewContext);
            return viewContext;
        }

        public static List<string> GetStyleBundle(this WebViewPage page)
        {
            var viewBag = GetTopViewContext(page.ViewContext).Controller.ViewBag;
            var bundle = viewBag._MsCms_StyleBundle as List<string>;
            if (bundle == null)
            {
                viewBag._MsCms_StyleBundle = bundle = new List<string>();
            }
            return bundle;
        }

        public static List<string> GetScriptBundle(this WebViewPage page)
        {
            var viewBag = GetTopViewContext(page.ViewContext).Controller.ViewBag;
            var scriptBundle = viewBag._MsCms_ScriptBundle as List<string>;
            if (scriptBundle == null)
            {
                viewBag._MsCms_ScriptBundle = scriptBundle = new List<string>();
            }
            return scriptBundle;
        }


        public static void RenderScene(this WebViewPage page, string sceneId, object model)
        {
            page.Html.RenderAction("Details", "Scene", new { sceneId, model });
            page.RenderStylesAndScripts();
        }

        public static void RenderDesignScene(this WebViewPage page, string sceneId, object model)
        {
            // save crrent URL in sesion so we know it when redirecting back from brick editing
            page.Session["_lastUsedSceneDesignUrl"] = page.Request.Url.AbsoluteUri;

            page.Html.RenderAction("Edit", "Scene", new { sceneId, model });

            page.RenderStylesAndScripts();
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
                var scripts = page.GetScriptBundle().Distinct().Select(s => "'" + page.Url.Content(s) + "'");
                page.Write(new MvcHtmlString("<script>require([" + string.Join(",",  scripts) + "])</script>"));
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