using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Entities;
using System.Web.Script.Serialization;
using System.Data.Objects.DataClasses;

namespace Bnh.Controllers
{
    [Authorize(Roles = "content_manager")]
    public class SceneController : Controller
    {
        private CmEntities db = new CmEntities();


        // GET: /Scene/Edit/5
        public ActionResult Edit(Guid id)
        {
            ViewBag.OwnerId = id;
            var walls = db.Walls.Where(w => w.OwnerId == id);
            ViewBag.Templates = new SelectList(db.SceneTemplates.ToList(), "Id", "Title");
            ViewBag.LockWalls = true;
            return View(walls);
        }

        // POST: /Scene/Edit/5

        [HttpPost]
        public ActionResult Save(Guid ownerId, List<Wall> walls)
        {
            if (ModelState.IsValid)
            {
                //ensure walls and bricks
                db.Refresh(RefreshMode.StoreWins, db.Walls);
                db.Refresh(RefreshMode.StoreWins, db.Bricks);

                // ensure walls collection is not null
                walls = walls ?? new List<Wall>();

                // get moved bricks
                var movedBricks = (from wall in walls
                                   from brick in wall.Bricks
                                   where wall.Id != brick.WallId && brick.Id != 0
                                   select brick.Id).ToList();

                // update existing walls
                foreach (var wall in walls.Where(w => w.Id != 0))
                {
                    var realWall = db.Walls.ApplyCurrentValues(wall);

                    // determine bricks removed from given wall
                    var bricksToRemove = realWall.Bricks
                        .Select(b => b.Id)
                        .Except(movedBricks)
                        .Except(wall.Bricks.Select(b => b.Id))
                        .ToList();
                    foreach (var bid in bricksToRemove)
                    {
                        db.Bricks.DeleteObject(db.Bricks.FirstOrDefault(b => b.Id == bid));
                    }

                    // add new bricks 
                    foreach (var brick in wall.Bricks.Where(b => b.Id == 0).ToList())
                    {
                        brick.Wall = realWall;
                    }

                    // updated bricks
                    foreach (var brick in wall.Bricks.Where(b => (b.Id != 0) && (!movedBricks.Contains(b.Id))))
                    {
                        ApplyCommonBrickValues(brick);
                    }

                    // moved bricks 
                    foreach (var brick in wall.Bricks.Where(b => movedBricks.Contains(b.Id)))
                    {
                        var realBrick = ApplyCommonBrickValues(brick);
                        realBrick.Wall = realWall;
                    }
                }

                // add new walls
                foreach (var wall in walls.Where(w => w.Id == 0))
                {
                    // moved bricks
                    foreach (var brick in wall.Bricks.Where(b => movedBricks.Contains(b.Id)).ToList())
                    {
                        wall.Bricks.Remove(brick);
                        var realBrick = ApplyCommonBrickValues(brick);
                        realBrick.Wall = wall;
                    }

                    wall.OwnerId = ownerId;
                    db.Walls.AddObject(wall);
                }

                // remove walls
                foreach (var realWall in db.Walls.Where(w => w.OwnerId == ownerId).ToList())
                {
                    var ids = walls.Where(w => w.Id != 0).Select(w => w.Id).ToList();
                    if (!ids.Contains(realWall.Id))
                    {
                        realWall.Bricks.ToList().ForEach(db.Bricks.DeleteObject);

                        db.Walls.DeleteObject(realWall);
                    }
                }

                db.SaveChanges();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("DesignScene", db.Walls.Where(w => w.OwnerId == ownerId));
            }

            return View("DesignScene");
        }

        // apply only properties that can be changed on scene designer
        private Brick ApplyCommonBrickValues(Brick brick)
        {
            var realBrick = db.Bricks.FirstOrDefault(b => b.Id == brick.Id);
            if (realBrick.Order != brick.Order)
                realBrick.Order = brick.Order;
            if (realBrick.Width != brick.Width)
                realBrick.Width = brick.Width;
            return realBrick;
        }

        [HttpPost]
        public ActionResult ExportTemplate(string title, Guid ownerId)
        {
            if (ModelState.IsValid)
            {
                var template = db.SceneTemplates.CreateObject();
                template.Id = Guid.NewGuid();
                template.Title = title;

                SceneTemplating.CloneScene(ownerId, template.Id, db);

                db.SceneTemplates.AddObject(template);

                db.SaveChanges();
            }

            return View("Empty");
        }

        [HttpPost]
        public ActionResult ApplyTemplate(Guid ownerId, Guid templateId)
        {
            // delete obsolete scene
            db.Walls.Where(w => w.OwnerId == ownerId).ToList().ForEach(db.Walls.DeleteObject);

            SceneTemplating.CloneScene(templateId, ownerId, db);

            db.SaveChanges();

            return PartialView("DesignScene", db.Walls.Where(w => w.OwnerId == ownerId));
        }

        [HttpPost]
        public ActionResult CanDeleteBrick(Brick brick)
        {
            return new JsonResult() { Data = !db.Bricks.OfType<SharedBrick>().Where(b => b.SharedBrickId == brick.Id).Any() };
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}