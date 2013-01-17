using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Core.Entities;
using System.Web;
using System.Web.Mvc;
using Bnh.Core.Entities.Attributes;

namespace Bnh
{
    public static class FilterPropertyAttributeExtensions
    {
        public static string GetJsOperator(this FilterPropertyAttribute attr)
        {
            switch(attr.Operator)
            {
                case FilterOperator.Equal:
                    return "===";

                case FilterOperator.Greater:
                    return ">";

                case FilterOperator.GreaterOrEqual:
                    return ">=";

                case FilterOperator.Less:
                    return "<";

                case FilterOperator.LessOrEqual:
                    return "<=";

                case FilterOperator.NotEqual:
                    return "!==";
            }

            throw new NotSupportedException("Given filter operator is not supported");
        }
    }
}
