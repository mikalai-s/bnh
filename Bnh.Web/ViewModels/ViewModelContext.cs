using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bnh.Web.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewModelContext
    {
        public UrlHelper UrlHelper { get; private set; }

        public HtmlHelper HtmlHelper { get; private set; }

        public bool IsUserContentManager { get; set; }

        public ViewModelContext(Controller controller)
        {
            this.UrlHelper = new UrlHelper(controller.HttpContext.Request.RequestContext);

            this.HtmlHelper = new HtmlHelper(new ViewContext(controller.ControllerContext, new WebFormView(
                controller.ControllerContext, "fake"), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()), new ViewPage());

            this.IsUserContentManager = controller.HttpContext.User.IsInRole("content_manager");
        }        
    }
}