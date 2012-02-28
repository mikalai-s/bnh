using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Entities;
using System.Web.Script.Serialization;
using System.Data.Objects.DataClasses;

namespace Bnh.Controllers
{ 
    public class WallController : Controller
    {
        private CmEntities db = new CmEntities();

        //
        // GET: /Wall/

        public ViewResult Index()
        {
            return View(db.Walls.ToList());
        }

        //
        // GET: /Wall/Details/5

        public ViewResult Details(long id)
        {
            Wall wall = db.Walls.Single(w => w.Id == id);
            return View(wall);
        }

        //
        // GET: /Wall/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Wall/Create

        [HttpPost]
        [Authorize(Roles="content_manager")]
        public ActionResult Create(Wall wall)
        {
            if (ModelState.IsValid)
            {
                db.Walls.AddObject(wall);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(wall);
        }
        
        //
        // GET: /Wall/Edit/5
        [Authorize(Roles="content_manager")]
        public ActionResult Edit(long id)
        {
            Wall wall = db.Walls.Single(w => w.Id == id);
            return View(wall);
        }

        //public ActionResult GetWallScene(Wall wall)
        //{
        //    return PartialView("WallScene", wall.Bricks);
        //}

        //public ActionResult GetWallScene(Community community)
        //{
        //    return PartialView("WallScene", community.GetWallBricks());
        //}

        //
        // POST: /Wall/Edit/5

        [HttpPost]
        [Authorize(Roles="content_manager")]
        // NOTE: Currently it's called asynchronously
        public ActionResult Edit(Wall wall, IEnumerable<Brick> bricks)
        {
            // TODO: use Request.IsAjaxRequest() to distinguish from usual postbacks

            if (ModelState.IsValid)
            {
                // get real wall
                var realWall = db.Walls.Single(w => w.Id == wall.Id);
                // delete existing bricks in real wall
                realWall.Bricks.ToList().ForEach(db.Bricks.DeleteObject);
                // apply client change to real wall
                db.Walls.ApplyCurrentValues(wall);
                // add new bricks to real wall
                if (bricks != null)
                {
                    bricks.ToList().ForEach(b => b.Wall = realWall);
                }
                // save changes
                db.SaveChanges();
                return PartialView("WallSceneDesigner", bricks);
            }
            return PartialView("WallSceneDesigner", wall.Bricks);
        }

        //
        // GET: /Wall/Delete/5
        [Authorize(Roles="content_manager")]
        public ActionResult Delete(long id)
        {
            Wall wall = db.Walls.Single(w => w.Id == id);
            return View(wall);
        }

        //
        // POST: /Wall/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles="content_manager")]
        public ActionResult DeleteConfirmed(long id)
        {            
            Wall wall = db.Walls.Single(w => w.Id == id);
            db.Walls.DeleteObject(wall);
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