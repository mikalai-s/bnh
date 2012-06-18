using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ms.Cms
{
    public class DesignerAuthorizeAttribute : AuthorizeAttribute
    {
        public DesignerAuthorizeAttribute()
        {
            this.Roles = MsCms.DesignerRole;
        }
    }
}