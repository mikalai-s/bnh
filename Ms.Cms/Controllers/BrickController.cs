using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ms.Cms.Models;

namespace Ms.Cms.Controllers
{
    [DesignerAuthorize]
    public class BrickController : Controller
    {
        private CmsEntities db = new CmsEntities();
       
        //
        // GET: /Brick/Edit/5
        public ActionResult Edit(string id)
        {
            var brick = db.BrickContents.First(b => b.Id == id);
            ViewBag.PartialViewSuffix = brick.GetType().Name;
            return View("~/WebExtracted/Ms.Cms/Views/Brick/Edit.cshtml", brick);
        }

        [HttpPost]
        public ActionResult Edit(BrickContent content)
        {
            var htmlcontent = content as HtmlContent;
            if (htmlcontent != null)
            {
                htmlcontent.Html = HttpUtility.HtmlDecode(htmlcontent.Html);
            }
            db.BrickContents.Save(content);

            // redirect to scene
            return RedirectToAction("Edit", "Scene", new { id = content.GetSceneId() });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}