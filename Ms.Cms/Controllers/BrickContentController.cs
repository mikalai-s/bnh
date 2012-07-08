using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ms.Cms.Models;

namespace Ms.Cms.Controllers
{
    [DesignerAuthorize]
    public class BrickContentController : Controller
    {
        private CmsEntities db = new CmsEntities();
       
        //
        // GET: /BrickContent/Edit/5
        public ActionResult Edit(string id)
        {
            var content = db.BrickContents.First(b => b.BrickContentId == id);
            return View(ContentUrl.Views.BrickContent.Edit, content);
        }

        [HttpPost]
        public ActionResult Edit(BrickContent content)
        {
            var htmlContent = content as HtmlContent;
            if (htmlContent != null)
            {
                htmlContent.Html = HttpUtility.HtmlDecode(htmlContent.Html);
            }
            db.BrickContents.Save(content);

            // redirect to previous url
            return Redirect(this.Request.RequestContext.GetBackUrl());
        }

        [HttpGet]
        public new ActionResult View(string id)
        {
            var content = db.BrickContents.First(b => b.BrickContentId == id);
            return View(ContentUrl.Views.BrickContent.View, content);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}