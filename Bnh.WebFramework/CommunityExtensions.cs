using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Entities;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Web;

namespace Bnh.WebFramework
{
    public static class CommunityExtensions
    {
        public static string GetClientProperties(this Community community)
        {
            var serializer = new JavaScriptSerializer();
            var clientProperties = new List<string>();
            var properties = typeof(Community).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var builder = new StringBuilder("{");
            foreach(var property in properties)
            {
                if (!Attribute.IsDefined(property, typeof(FilterablePropertyAttribute)))
                {
                    continue;
                }

                var name = property.Name[0].ToString().ToLower() + property.Name.Substring(1);
                var value = property.GetValue(community, null);

                builder.AppendFormat("{0}:{1},", name, serializer.Serialize(value));
            }

            builder.Append("}");

            return builder.ToString();
        }
    }
}
