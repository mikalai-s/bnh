using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bnh.Cms.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewModelContext
    {
        protected Controller controller { get; private set; }

        public UrlHelper UrlHelper { get; private set; }

        public HtmlHelper HtmlHelper { get; private set; }

        public ViewModelContext(Controller controller)
        {
            this.controller = controller;

            this.UrlHelper = new UrlHelper(controller.HttpContext.Request.RequestContext);

            this.HtmlHelper = new HtmlHelper(new ViewContext(controller.ControllerContext, new WebFormView(
                controller.ControllerContext, "fake"), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()), new ViewPage());
        }

        public virtual bool IsUserInRole(string role)
        {
            return this.controller.HttpContext.User.IsInRole(role);
        }
    }
}