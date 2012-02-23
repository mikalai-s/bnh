
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Bnh.WebFramework
{
    // Summary:
    //     Represents support for HTML links in an application.
    public static class HtmlExtensions
    {
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

        public static MvcHtmlString ActionInputLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            var inputBuilder = new TagBuilder("input");
            inputBuilder.Attributes["value"] = linkText;
            inputBuilder.Attributes["type"] = "button";
            

            string href = UrlHelper.GenerateUrl(null, actionName, controllerName, null, null, null, routeValues, htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true);
            inputBuilder.Attributes["onclick"] = "location.href='" + href + "'";
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
    }
}
