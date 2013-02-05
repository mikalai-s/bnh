using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Core;

namespace Cms.Infrastructure
{
    public class DesignerAuthorizeAttribute : AuthorizeAttribute
    {
        public IConfig Config { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            this.Roles = this.Config.Roles["ContentManager"];
            return base.AuthorizeCore(httpContext);
        }
    }
}