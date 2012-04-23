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
            foreach (var jsp in JsProperty.Get(typeof(Community)))
            {
                var value = jsp.Property.GetValue(community, null);

                builder.AppendFormat("{0}:{1},", jsp.Name, serializer.Serialize(value));
            }

            builder.Append("}");

            return builder.ToString();
        }

        public class JsProperty
        {
            public IHtmlString Name { get; private set; }
            public IHtmlString Title { get; private set; }
            public IHtmlString Operator { get; private set; }
            
            public PropertyInfo Property { get; private set; }            

            public static IEnumerable<JsProperty> Get(Type type)
            {
                return from property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       where Attribute.IsDefined(property, typeof(FilterablePropertyAttribute))
                       let name = property.Name[0].ToString().ToLower() + property.Name.Substring(1)
                       let attr = (property.GetCustomAttributes(typeof(FilterablePropertyAttribute), false).First() as FilterablePropertyAttribute)
                       select new JsProperty
                       {
                           Name = new MvcHtmlString(name),
                           Title = new MvcHtmlString(attr.Title),
                           Operator = attr.GetJsOperator(),
                           Property = property
                       };
            }
        }        
    }
}
