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

        //
        // GET: /Brick/

        public ViewResult Index()
        {
            var bricks = db.Bricks.Include("Wall");
            return View(bricks.ToList());
        }

        //
        // GET: /Brick/Details/5

        public ViewResult Details(long id)
        {
            Brick brick = db.Bricks.Single(b => b.Id == id);
            return View(brick);
        }

        //
        // GET: /Brick/Create

        public ActionResult Create()
        {
            ViewBag.WallId = new SelectList(db.Walls, "Id", "Title");
            return View();
        } 

        //
        // POST: /Brick/Create

        [HttpPost]
        public ActionResult Create(Brick brick)
        {
            if (ModelState.IsValid)
            {
                db.Bricks.AddObject(brick);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.WallId = new SelectList(db.Walls, "Id", "Title", brick.WallId);
            return View(brick);
        }
        
        //
        // GET: /Brick/Edit/5
 
        public ActionResult Edit(long id)
        {
            Brick brick = db.Bricks.Single(b => b.Id == id);
            ViewBag.WallId = new SelectList(db.Walls, "Id", "Title", brick.WallId);
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
                return RedirectToAction("Index");
            }
            ViewBag.WallId = new SelectList(db.Walls, "Id", "Title", brick.WallId);
            return View(brick);
        }

        //
        // GET: /Brick/Delete/5
 
        public ActionResult Delete(long id)
        {
            Brick brick = db.Bricks.Single(b => b.Id == id);
            return View(brick);
        }

        //
        // POST: /Brick/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            Brick brick = db.Bricks.Single(b => b.Id == id);
            db.Bricks.DeleteObject(brick);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}