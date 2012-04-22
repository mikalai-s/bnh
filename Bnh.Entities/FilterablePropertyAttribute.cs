using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh.Entities
{
    public enum FilterOperator
    {
        Equal,
        Greater,
        GreaterOrEqual,
        Less,
        LessOrEqual,
        NotEqual
    }

    public class FilterablePropertyAttribute : Attribute
    {
        public string Title { get; set; }

        public FilterOperator Operator { get; set; }

        public FilterablePropertyAttribute ()
        {
            Operator = FilterOperator.Equal;
        }
    }
}
