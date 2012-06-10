
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

namespace Ms.Cms.Web
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
                                                                         {typeof(SharedBrick), "Shared"},
                                                                         {typeof(TocBrick), "Table of Content"}
                                                                     };

        public static string GetBrickTypeName(Type brickType)
        {
            return BrickTypeNames[brickType];
        }

        public static MvcHtmlString DropDownListForBrickTypes(this HtmlHelper htmlHelper, string name)
        {
                var items = BnhModelBinder.HierarchyTypeMap[typeof(Brick)]
                .Select(e => new SelectListItem { Value = e.Key, Text = BrickTypeNames[e.Value] });
                return htmlHelper.DropDownList(name, items);
            }
        }
    }
