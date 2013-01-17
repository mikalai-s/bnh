using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Web.Mvc;
using Bnh.Core.Entities.Attributes;

namespace Bnh
{
    public class ExternalProperty
    {
        public string Name { get; protected set; }
        public string Title { get; protected set; }
        public ExternalPropertyAttribute Attribute { get; protected set; }

        public PropertyInfo Property { get; protected set; }

        public string TypeName
        {
            get { return this.Property.PropertyType.Name; }
        }

        public static IEnumerable<ExternalProperty> Get(Type type)
        {
            return from property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   where System.Attribute.IsDefined(property, typeof(ExternalPropertyAttribute))
                   let name = property.Name[0].ToString().ToLower() + property.Name.Substring(1)
                   let attribute = (property.GetCustomAttributes(typeof(ExternalPropertyAttribute), false).First() as ExternalPropertyAttribute)
                   select new ExternalProperty
                   {
                       Name = name,
                       Title = attribute.Title ?? property.Name,
                       Attribute = attribute,
                       Property = property
                   };
        }
    }

    public class FilterProperty : ExternalProperty
    {
        public string Operator { get; private set; }
        public new FilterPropertyAttribute Attribute { get; protected set; }
        public object Default { get; protected set; } 

        public static new IEnumerable<FilterProperty> Get(Type type)
        {
            return from property in ExternalProperty.Get(type)
                   let attribute = property.Attribute as FilterPropertyAttribute
                   where attribute != null
                   select new FilterProperty
                   {
                       Name = property.Name,
                       Title = property.Title,
                       Attribute = attribute,
                       Property = property.Property,
                       Operator = attribute.GetJsOperator(),
                       Default = attribute.Default ?? (property.Property.PropertyType.IsValueType ? Activator.CreateInstance(property.Property.PropertyType) : null)
                   };
        }
    }
}
