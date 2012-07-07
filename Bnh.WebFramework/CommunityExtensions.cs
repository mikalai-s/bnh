using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Entities;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Web.Routing;

namespace Bnh.WebFramework
{
    public static class CommunityExtensions
    {
        public static string GetClientProperties(this Community community)
        {
            var properties = new Dictionary<string, object>();
            foreach (var jsp in FilterProperty.Get(typeof(Community)))
            {
                properties[jsp.Name.ToHtmlString()] = jsp.Property.GetValue(community, null);
            }
            return JsonConvert.SerializeObject(properties);
        }

        public static string GetReturnUrl(this Community community)
        {
            return UrlHelper.GenerateUrl(null, "View", "Community", new RouteValueDictionary(new { id = community.UrlId }), new RouteCollection(), HttpContext.Current.Request.RequestContext, false);
        }


    }
}
