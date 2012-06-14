using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms.Models.Attributes
{
    /// <summary>
    /// Proerties marked with this attribute are getting cloned
    /// </summary>
    public class NonCloneableAttribute : System.Attribute
    {
    }
}