using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ms.Cms
{
    internal static class UrlExtensions
    {
        public static string Resolve(this UrlHelper url, string contentPath)
        {
            return "~" + MsCms.WebExtractorUrl + contentPath;
        }
    }
}