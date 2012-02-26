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
    public class WallTestController : Controller
    {
        private CmEntities db = new CmEntities();

        //
        // GET: /WallTest/

        public ViewResult Index()
        {
            return View(db.Walls.ToList());
        }

        //
        // GET: /WallTest/Details/5

        public ViewResult Details(long id)
        {
            Wall wall = db.Walls.Single(w => w.Id == id);
            return View(wall);
        }

        //
        // GET: /WallTest/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /WallTest/Create

        [HttpPost]
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
        // GET: /WallTest/Edit/5
        
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
        // POST: /WallTest/Edit/5

        [HttpPost]
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
                return PartialView("WallScene", bricks);
            }
            return PartialView("WallScene", wall.Bricks);
        }

        //
        // GET: /WallTest/Delete/5
 
        public ActionResult Delete(long id)
        {
            Wall wall = db.Walls.Single(w => w.Id == id);
            return View(wall);
        }

        //
        // POST: /WallTest/Delete/5

        [HttpPost, ActionName("Delete")]
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