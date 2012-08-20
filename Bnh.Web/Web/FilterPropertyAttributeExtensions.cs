using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Core.Entities;
using System.Web;
using System.Web.Mvc;
using Bnh.Core.Entities.Attributes;

namespace Bnh.WebFramework
{
    public static class FilterPropertyAttributeExtensions
    {
        public static IHtmlString GetJsOperator(this FilterPropertyAttribute attr)
        {
            switch(attr.Operator)
            {
                case FilterOperator.Equal:
                    return new MvcHtmlString("===");

                case FilterOperator.Greater:
                    return new MvcHtmlString(">");

                case FilterOperator.GreaterOrEqual:
                    return new MvcHtmlString(">=");

                case FilterOperator.Less:
                    return new MvcHtmlString("<");

                case FilterOperator.LessOrEqual:
                    return new MvcHtmlString("<=");

                case FilterOperator.NotEqual:
                    return new MvcHtmlString("!==");
            }

            throw new NotSupportedException("Given filter operator is not supported");
        }
    }
}
