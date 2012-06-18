using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        private Dictionary<string, string> BrickEditView = new Dictionary<string, string>
        {
            {"EmptyBrick", "~/WebExtracted/Ms.Cms/Views/Brick/Edit.cshtml"},
            {"HtmlBrick", "~/WebExtracted/Ms.Cms/Views/Brick/EditHtml.cshtml"},
            {"GalleryBrick", "~/WebExtracted/Ms.Cms/Views/Brick/EditGallery.cshtml"},
            {"MapBrick", "~/WebExtracted/Ms.Cms/Views/Brick/EditMap.cshtml"},
            {"RazorBrick", "~/WebExtracted/Ms.Cms/Views/Brick/EditHtml.cshtml"},
            {"LinkableBrick", "~/WebExtracted/Ms.Cms/Views/Brick/EditLinkable.cshtml"},
        };
        
        //
        // GET: /Brick/Edit/5
        public ActionResult Edit(long id)
        {
            Brick brick = db.Bricks.Single(b => b.Id == id);
            ViewBag.WallId = new SelectList(db.Walls, "Id", "Title", brick.Wall.Id);
            ViewBag.PartialView = BrickEditView[brick.GetDiscriminant()];
            return View("~/WebExtracted/Ms.Cms/Views/Brick/Edit.cshtml", brick);
        }

        [HttpPost]
        public ActionResult Edit(Brick brick)
        {
            // update current brick
            db.Bricks.Attach(brick);
            db.Entry(brick).State = EntityState.Modified;
            db.SaveChanges();

            // load brick's wall property to get current scene later
            db.Entry(brick).Reference(b => b.Wall).Load();

            // redirect to scene
            return RedirectToAction("Edit", "Scene", new { id = brick.Wall.SceneId });
        }

        [HttpPost]
        public ActionResult EditHtml(HtmlBrick brick)
        {
            brick.Html = HttpUtility.HtmlDecode(brick.Html);
            return Edit(brick);
        }

        [HttpPost]
        public ActionResult EditMap(MapBrick brick)
        {
            return Edit(brick);
        }

        [HttpPost]
        public ActionResult EditGallery(GalleryBrick brick)
        {
            return Edit(brick);
        }

        [HttpPost]
        public ActionResult EditLinkable(LinkableBrick brick)
        {
            return Edit(brick);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}