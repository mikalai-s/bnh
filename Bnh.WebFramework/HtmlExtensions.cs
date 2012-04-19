
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Bnh.Entities;
using System;
using System.Reflection;
using System.Text;

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

            if(htmlAttributes != null)
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

        private static readonly Dictionary<Type, string> BrickTypeNames = new Dictionary<Type, string>()
                                                                     {
                                                                         {typeof(HtmlBrick), "Rich Text"},
                                                                         {typeof(GalleryBrick), "Gallery"},
                                                                         {typeof(MapBrick), "Map"},
                                                                         {typeof(EmptyBrick), "Empty"},
                                                                     };

        public static MvcHtmlString DropDownListForBrickTypes(this HtmlHelper htmlHelper, string name)
        {
            var items = BnhModelBinder.HierarchyTypeMap[typeof (Brick)]
                .Select(e => new SelectListItem { Value = e.Key, Text = BrickTypeNames[e.Value] });
            return htmlHelper.DropDownList(name, items);
        }


        public static MvcHtmlString CommunityFilter(this HtmlHelper htmlHelper)
        {
            var builder = new StringBuilder();
            var scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine("<script type=\"text/javascript\">");
            scriptBuilder.AppendLine("    function communityFilterViewModel() {");
            scriptBuilder.AppendLine("        var self = this;");

            var properties = typeof(Community).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (!Attribute.IsDefined(property, typeof(FilterablePropertyAttribute)))
                {
                    continue;
                }

                var csName = property.Name;
                var jsName = csName[0].ToString().ToLower() + csName.Substring(1);

                if (property.PropertyType == typeof(bool))
                {
                    builder.AppendFormat("<div><input id=\"hasLake\" type=\"checkbox\" class=\"filter\" data-bind=\"checked: {0}\" /> {1}</div>\r\n",
                        jsName, csName);

                    scriptBuilder.AppendFormat("        self.{0} = ko.observable(false);\r\n", jsName);
                }
            }
            
            scriptBuilder.AppendLine("    }");
            scriptBuilder.AppendLine("</script>");

            return MvcHtmlString.Create(builder.ToString() + Environment.NewLine + scriptBuilder.ToString());
        }
    }
}
