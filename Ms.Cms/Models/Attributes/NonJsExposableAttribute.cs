using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms.Models.Attributes
{
    /// <summary>
    /// Properties marked with this attribute are getting exposed to client during JSON serialization
    /// </summary>
    public class NonJsExposableAttribute : System.Attribute
    {
    }
}