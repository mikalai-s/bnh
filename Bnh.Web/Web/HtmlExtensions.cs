
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Microsoft.Web.Helpers;

namespace Bnh.WebFramework
{
    // Summary:
    //     Represents support for HTML links in an application.
    public static class HtmlExtensions
    {
        public static MvcHtmlString Avatar(this HtmlHelper html, string userName, int size = 64)
        {
            return new MvcHtmlString(Gravatar.GetUrl(userName, size) + "&d=identicon");
        }

        public static MvcHtmlString Rating(this HtmlHelper html, int rating)
        {
            var format = "<div class='scale l' style='width:{0}px'></div><div class='scale r' style='width:{1}px'></div>";
            return new MvcHtmlString(string.Format(format, rating * 10, 100 - rating * 10));
        }

        public static MvcHtmlString ActionMenuItem(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            var tag = new TagBuilder("li");

            if (htmlHelper.ViewContext.RequestContext.IsCurrentRoute(null, controllerName, actionName))
            {
                tag.AddCssClass("current");
            }

            tag.InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToString();

            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString ActionInputLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, IDictionary<string, object> htmlAttributes)
        {
            return ActionInputLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), htmlAttributes);
        }

        public static MvcHtmlString ActionInputLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            var inputBuilder = new TagBuilder("input");
            inputBuilder.Attributes["value"] = linkText;
            inputBuilder.Attributes["type"] = "button";


            string href = UrlHelper.GenerateUrl(null, actionName, controllerName, null, null, null, routeValues, htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true);
            inputBuilder.Attributes["onclick"] = "location.href='" + href + "'";

            if (htmlAttributes != null)
            {
                foreach (var attr in htmlAttributes)
                {
                    inputBuilder.Attributes[attr.Key] = attr.Value.ToString();
                }
            }
            //var linkBuilder = GetLinkBuilder(linkText, htmlAttributes, href);

            //linkBuilder.InnerHtml = inputBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(inputBuilder.ToString(TagRenderMode.Normal)); ;
        }

        private static TagBuilder GetLinkBuilder(string linkText, IDictionary<string, object> htmlAttributes, string href)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.InnerHtml = !string.IsNullOrEmpty(linkText) ? HttpUtility.HtmlEncode(linkText) : string.Empty;
            builder.MergeAttributes<string, object>(htmlAttributes);
            builder.MergeAttribute("href", href);
            return builder;
        }

        public static MvcHtmlString ActionInputLink(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            return ActionInputLink(htmlHelper, linkText, actionName, null, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString ActionInputLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return ActionInputLink(htmlHelper, linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public static IHtmlString Script(this HtmlHelper htmlHelper, String path)
        {
            TagBuilder tagBuilder = new TagBuilder("script");
            tagBuilder.MergeAttribute("src", path);
            tagBuilder.MergeAttribute("type", "text/javascript");
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}
