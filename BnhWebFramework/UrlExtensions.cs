// -----------------------------------------------------------------------
// <copyright file="UrlExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Web.Mvc;
using System.Web.Routing;

namespace BnhWebFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class UrlExtensions
    {
        public static bool IsCurrentRoute(this RequestContext context, string areaName, string controllerName, params string[] actionNames)
        {
            var routeData = context.RouteData;
            var routeArea = routeData.DataTokens["area"] as string;

            if (((string.IsNullOrEmpty(routeArea) && string.IsNullOrEmpty(areaName)) ||
                  (routeArea == areaName)) &&
                 ((string.IsNullOrEmpty(controllerName)) ||
                  (routeData.GetRequiredString("controller") == controllerName)) &&
                 ((actionNames == null) ||
                   actionNames.ToArray().Contains(routeData.GetRequiredString("action"))))
            {
                return true;
            }

            return false;
        }

        public static bool IsCurrent(this UrlHelper urlHelper, string areaName, string controllerName, params string[] actionNames)
        {
            return urlHelper.RequestContext.IsCurrentRoute(areaName, controllerName, actionNames);
        }

        public static string Selected(this UrlHelper urlHelper, string areaName, string controllerName, params string[] actionNames)
        {
            return urlHelper.IsCurrent(areaName, controllerName, actionNames) ? "selected" : string.Empty;
        }
    }
}
