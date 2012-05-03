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

    public class FilterPropertyAttribute : ExternalPropertyAttribute
    {
        public FilterOperator Operator { get; set; }

        public FilterPropertyAttribute()
        {
            Operator = FilterOperator.Equal;
        }
    }
}
