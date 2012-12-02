using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bnh.Web.ViewModels
{
    public abstract class PageViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        protected UrlHelper UrlHelper { get; private set; }

        protected HtmlHelper HtmlHelper { get; private set; }

        public PageViewModel(Controller controller)
        {
            this.UrlHelper = new UrlHelper(controller.HttpContext.Request.RequestContext);

            this.HtmlHelper = new HtmlHelper(new ViewContext(controller.ControllerContext, new WebFormView(
                controller.ControllerContext, "fake"), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()), new ViewPage());
        }
    }
}