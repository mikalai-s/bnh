using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Entities;
using Bnh.WebFramework;

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
        };
        
        //
        // GET: /Brick/Edit/5
        public ActionResult Edit(long id)
        {
            Brick brick = db.Bricks.Single(b => b.Id == id);
            ViewBag.WallId = new SelectList(db.Walls, "Id", "Title", brick.WallId);
            ViewBag.PartialView = BrickEditView[brick.GetType()];
            return View(brick);
        }

        [HttpPost]
        public ActionResult Edit(Brick brick)
        {
            var realBrick = db.Bricks.FirstOrDefault(b => b.Id == brick.Id);
            realBrick.Title = brick.Title;
            db.SaveChanges();
            return RedirectToAction("Edit", "Scene", new { id = realBrick.Wall.OwnerId });
        }

        [HttpPost]
        public ActionResult EditHtml(HtmlBrick brick)
        {
            var realBrick = db.Bricks.Where(b => b.Id == brick.Id).OfType<HtmlBrick>().FirstOrDefault();
            realBrick.Html = HttpUtility.HtmlDecode(brick.Html);
            return Edit(brick);
        }

        [HttpPost]
        public ActionResult EditMap(MapBrick brick)
        {
            var realBrick = db.Bricks.Where(b => b.Id == brick.Id).OfType<MapBrick>().FirstOrDefault();
            realBrick.GpsLocation = brick.GpsLocation;
            realBrick.Height = brick.Height;
            realBrick.Zoom = brick.Zoom;
            return Edit(brick);
        }

        [HttpPost]
        public ActionResult EditGallery(GalleryBrick brick)
        {
            var realBrick = db.Bricks.Where(b => b.Id == brick.Id).OfType<GalleryBrick>().FirstOrDefault();
            realBrick.ImageListId = brick.ImageListId;
            return Edit(brick);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}