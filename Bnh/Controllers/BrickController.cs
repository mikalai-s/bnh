using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Entities;

namespace Bnh.Controllers
{ 
    public class BrickController : Controller
    {
        private CmEntities db = new CmEntities();

        private Dictionary<Type, string> BrickEditView = new Dictionary<Type, string>
        {
            {typeof(Brick), "Edit"},
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

        
        //
        // POST: /Brick/Edit/5

        [HttpPost]
        public ActionResult Edit(Brick brick)
        {
            if (ModelState.IsValid)
            {
                db.Bricks.Attach(brick);
                db.ObjectStateManager.ChangeObjectState(brick, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Edit", "Wall", new { id = brick.WallId });
            }
            ViewBag.WallId = new SelectList(db.Walls, "Id", "Title", brick.WallId);
            return View(brick);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}