
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Microsoft.Web.Helpers;

namespace Bnh
{
    // Summary:
    //     Represents support for HTML links in an application.
    public static class HtmlExtensions
    {
        private static readonly object lastFormNumKey;

        static HtmlExtensions()
        {
            lastFormNumKey = new object();
        }

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

        public static MvcHtmlString ActionInputLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName = null, object routeValues = null, object attributes = null)
        {
            var inputBuilder = new TagBuilder("input");
            inputBuilder.Attributes["value"] = linkText;
            inputBuilder.Attributes["type"] = "button";

            var attributesDictionary = ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));

            string href = UrlHelper.GenerateUrl(null, actionName, controllerName, null, null, null, new RouteValueDictionary(routeValues), htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true);
            inputBuilder.Attributes["onclick"] = "location.href='" + href + "'";

            if (attributes != null)
            {
                foreach (var attr in attributesDictionary)
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

        public static IHtmlString Script(this HtmlHelper htmlHelper, String path)
        {
            TagBuilder tagBuilder = new TagBuilder("script");
            tagBuilder.MergeAttribute("src", path);
            tagBuilder.MergeAttribute("type", "text/javascript");
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public static MvcForm BeginForm(this HtmlHelper helper, string formAction, FormMethod method = FormMethod.Post, object attributes = null)
        {
            TagBuilder builder = new TagBuilder("form");
            builder.MergeAttributes<string, object>(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            builder.MergeAttribute("action", formAction);
            builder.MergeAttribute("method", HtmlHelper.GetFormMethodString(method), true);
            bool flag = helper.ViewContext.ClientValidationEnabled && !helper.ViewContext.UnobtrusiveJavaScriptEnabled;
            if (flag)
            {

                builder.GenerateId(FormIdGenerator(helper));
            }
            helper.ViewContext.Writer.Write(builder.ToString(TagRenderMode.StartTag));
            MvcForm form = new MvcForm(helper.ViewContext);
            if (flag)
            {
                helper.ViewContext.FormContext.FormId = builder.Attributes["id"];
            }
            return form;
        }

        private static string FormIdGenerator(HtmlHelper helper)
        {
            int num = IncrementFormCount(helper.ViewContext.HttpContext.Items);
            return string.Format(CultureInfo.InvariantCulture, "form{0}", new object[] { num });
        }

        private static int IncrementFormCount(IDictionary items)
        {
            object obj2 = items[lastFormNumKey];
            int num = (obj2 != null) ? (((int)obj2) + 1) : 0;
            items[lastFormNumKey] = num;
            return num;
        }

        /// <summary>
        /// Creates rating stars HTML, where rating is in percents
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        public static MvcHtmlString RatingStars(this HtmlHelper helper, double? rating)
        {
            if (rating.HasValue)
            {
                return new MvcHtmlString("<div class='scale'><div class='l' style='width:{0}%'></div></div>".FormatWith(rating * 100));
            }

            return new MvcHtmlString("Not rated");
        }
    }
}
