using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Web.ViewModels
{
    public class FilterPropertyViewModel
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Comparer { get; set; }

        public object DefaultValue { get; set; }

        public string Type { get; set; }

        public static IEnumerable<FilterPropertyViewModel> EnumerateProperties(Type type)
        {
            return from jsp in FilterProperty.Get(type)
                   select new FilterPropertyViewModel
                   {
                       Name = jsp.Property.Name,
                       Title = jsp.Title,
                       Comparer = "(function(a, b) {{ return a {0} b; }})".FormatWith(jsp.Operator),
                       DefaultValue = jsp.Default,
                       Type = jsp.Property.PropertyType.Name
                   };
        }
    }
}