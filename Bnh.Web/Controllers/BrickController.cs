using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Entities;
using Bnh.WebFramework;

using Ms.Cms.Models;

namespace Bnh.Controllers
{
    [Authorize(Roles = "content_manager")]
    public class BrickController : Controller
    {
        private CmEntities db = new CmEntities();

        private Dictionary<Type, string> BrickEditView = new Dictionary<Type, string>
        {
            {typeof(EmptyBrick), "Edit"},
            {typeof(HtmlBrick), "EditHtml"},
            {typeof(GalleryBrick), "EditGallery"},
            {typeof(MapBrick), "EditMap"},
            {typeof(RazorBrick), "EditHtml"},
            {typeof(LinkableBrick), "EditLinkable"},
        };
        
        //
        // GET: /Brick/Edit/5
        public ActionResult Edit(long id)
        {
            Brick brick = db.Bricks.Single(b => b.Id == id);
            ViewBag.WallId = new SelectList(db.Walls, "Id", "Title", brick.Wall.Id);
            ViewBag.PartialView = BrickEditView[brick.GetType()];
            return View(brick);
        }

        [HttpPost]
        public ActionResult Edit(Brick brick)
        {
            db.Bricks.Attach(brick);
            //db.ObjectStateManager.ChangeObjectState(brick, EntityState.Modified);
            db.SaveChanges();
            return RedirectToAction("Edit", "Scene", new { id = brick.Wall.OwnerId });
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