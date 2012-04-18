using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Entities;
using System.Reflection;
using System.Web.Script.Serialization;

namespace Bnh.WebFramework
{
    public static class CommunityExtensions
    {
        public static string IsVisibleKoExpression(this Community community)
        {
            var builder = new StringBuilder();
            var properties = typeof(Community).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var property in properties)
            {
                if (!Attribute.IsDefined(property, typeof(FilterablePropertyAttribute)))
                {
                    continue;
                }

                var name = property.Name.First().ToString().ToLower() + string.Join("", property.Name.Skip(1));
                var value = property.GetValue(community, null);
                var serializer = new JavaScriptSerializer();

                builder.AppendFormat("(!{0}() || {0}()==={1})&&", name, serializer.Serialize(value));
            }
            builder.Append("true");
            return builder.ToString();
        }
    }
}
