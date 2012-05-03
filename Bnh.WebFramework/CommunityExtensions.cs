using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Entities;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace Bnh.WebFramework
{
    public static class CommunityExtensions
    {
        public static string GetClientProperties(this Community community)
        {
            var serializer = new JavaScriptSerializer();
            var clientProperties = new List<string>();
            var builder = new StringBuilder("{");
            foreach (var jsp in FilterProperty.Get(typeof(Community)))
            {
                var value = jsp.Property.GetValue(community, null);

                builder.AppendFormat("{0}:{1},", jsp.Name, serializer.Serialize(value));
            }

            builder.Append("}");

            return builder.ToString();
        }
    }
}
