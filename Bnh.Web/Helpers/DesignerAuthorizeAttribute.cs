using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bnh
{
    public class DesignerAuthorizeAttribute : AuthorizeAttribute
    {
        public DesignerAuthorizeAttribute()
        {
            this.Roles = "content_manager";
        }
    }
}