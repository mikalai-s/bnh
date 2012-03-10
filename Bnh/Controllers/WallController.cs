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


        // GET: /Wall/Edit/5
        [Authorize(Roles="content_manager")]
        public ActionResult Edit(Guid id)
        {
            ViewBag.OwnerId = id;
            var walls = db.Walls.Where(w => w.OwnerId == id);
            return View(walls);
        }

        // POST: /Wall/Edit/5

        [HttpPost]
        [Authorize(Roles="content_manager")]
        public ActionResult Edit(Guid ownerId, List<Wall> walls)
        {
            if (ModelState.IsValid)
            {
                // update existing
                foreach(var wall in walls.Where(w => w.Id != 0))
                {
                    db.Walls.ApplyCurrentValues(wall);
                }

                // remove
                foreach (var realWall in db.Walls.Where(w => w.OwnerId == ownerId))
                {
                    var ids = walls.Where(w => w.Id != 0).Select(w => w.Id).ToList();
                    if (!ids.Contains(realWall.Id))
                    {
                        db.Walls.DeleteObject(realWall);
                    }
                }

                // add new walls (and bricks??????)
                foreach (var wall in walls.Where(w => w.Id == 0))
                {
                    wall.OwnerId = ownerId;
                    db.AddToWalls(wall);
                }

                db.SaveChanges();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("WallSceneDesigner", db.Walls.Where(w => w.OwnerId == ownerId));
            }
            
            return View("WallSceneDesigner");//, wall.Bricks);
        }

        private Wall SaveWall(Wall wall, List<Brick> added, List<Brick> edited, List<Brick> deleted)
        {
            // get real wall
            var realWall = db.Walls.Single(w => w.Id == wall.Id);

            // apply client change to real wall
            db.Walls.ApplyCurrentValues(wall);

            // for edited brick we want to update some properties
            if (edited != null)
            {
                foreach (var freshBrick in edited)
                {
                    var realBrick = realWall.Bricks.FirstOrDefault(b => b.Id == freshBrick.Id);

                    // on wall designer we update only brick common properties 
                    // that are editable on this screen
                    ApplyNewBrickProperties(freshBrick, realBrick);
                }
            }

            if (deleted != null)
            {
                // delete bricks in real wall
                var realBricks = from realBrick in realWall.Bricks
                                 from brickToDelete in deleted
                                 where realBrick.Id == brickToDelete.Id
                                 select realBrick;
                realBricks.ToList().ForEach(db.Bricks.DeleteObject);
            }

            if (added != null)
            {
                // add new bricks to real wall
                added.ForEach(b => b.Wall = realWall);
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}