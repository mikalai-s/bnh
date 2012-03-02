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
        public ActionResult Edit(Wall wall, List<Brick> add, List<Brick> edit, List<Brick> delete)
        {
            if (ModelState.IsValid)
            {
                wall = SaveWall(wall, add, edit, delete);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("WallSceneDesigner", wall.Bricks);
            }
            
            return View("WallSceneDesigner", wall.Bricks);
        }

        private Wall SaveWall(Wall wall, List<Brick> add, List<Brick> edit, List<Brick> delete)
        {
            // get real wall
            var realWall = db.Walls.Single(w => w.Id == wall.Id);

            // apply client change to real wall
            db.Walls.ApplyCurrentValues(wall);

            // for edited brick we want to update some properties
            if (edit != null)
            {
                foreach (var freshBrick in edit)
                {
                    var realBrick = realWall.Bricks.FirstOrDefault(b => b.Id == freshBrick.Id);

                    // on wall designer we update only brick common properties 
                    // that are editable on this screen
                    ApplyNewBrickProperties(freshBrick, realBrick);
                }
            }

            if (delete != null)
            {
                // delete bricks in real wall
                var realBricks = from realBrick in realWall.Bricks
                                 from brickToDelete in delete
                                 where realBrick.Id == brickToDelete.Id
                                 select realBrick;
                realBricks.ToList().ForEach(db.Bricks.DeleteObject);
            }

            if (add != null)
            {
                // add new bricks to real wall
                add.ForEach(b => b.Wall = realWall);
            }
            // save changes
            db.SaveChanges();
            return realWall;
        }

        private void ApplyNewBrickProperties(Brick source, Brick target)
        {
            target.Title = source.Title;
            target.Order = source.Order;
            target.Width = source.Width;
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