using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.Helpers;

namespace Bnh.Web.Helpers
{
    public class AjaxAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // Only do something if we are about to give a HttpUnauthorizedResult and we are in AJAX mode.
            if (filterContext.Result is HttpUnauthorizedResult && filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var builder = UrlBuilder.Create(FormsAuthentication.LoginUrl);
                builder.AddParam("ReturnUrl", filterContext.RequestContext.HttpContext.Request.UrlReferrer.PathAndQuery);

                filterContext.Result = new JavaScriptResult()
                {
                    Script = "window.location='" + builder.ToString() + "';"
                };
            }
        }
    }
}