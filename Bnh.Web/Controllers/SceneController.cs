using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ms.Cms.Models;


namespace Bnh.Controllers
{
    [Authorize(Roles = "content_manager")]
    public class SceneController : Controller
    {
        private CmsEntities db = new CmsEntities();


        // GET: /Scene/Edit/5
        public ActionResult Edit(long id)
        {
            ViewBag.OwnerId = id;
            var scene = db.Scenes.FirstOrDefault(s => s.Id == id);
            ViewBag.Templates = new SelectList(db.SceneTemplates.ToList(), "Id", "Title");
            ViewBag.LockWalls = true;
            return View(scene);
        }

        // POST: /Scene/Edit/5

        [HttpPost]
        public ActionResult Save(Scene scene)
        {
            if (ModelState.IsValid)
            {
                foreach (var wall in scene.Walls)
                {
                    foreach (var brick in wall.Bricks)
                    {
                        brick.Wall = wall;
                        db.Bricks.Attach(brick);
                        db.Entry(brick).State = brick.Id == 0 ? EntityState.Added : EntityState.Modified;
                    }

                    wall.Scene = scene;
                    db.Walls.Attach(wall);
                    db.Entry(wall).State = wall.Id == 0 ? EntityState.Added : EntityState.Modified;
                }

                scene = db.Scenes.Attach(scene);
                db.Entry(scene).State = EntityState.Modified;
                /*
                var realScene = db.Scenes.First(s => s.Id == scene.Id);

                // ensure walls collection is not null
                var walls = realScene.Walls ?? new List<Wall>();

                // get moved bricks
                var movedBricks = (from wall in walls
                                   from brick in wall.Bricks
                                   where wall.Id != brick.WallId && brick.Id != 0
                                   select brick.Id).ToList();

                // update existing walls
                foreach (var wall in walls.Where(w => w.Id != 0))
                {
                    var realWall = wall;// db.Walls.ApplyCurrentValues(wall);

                    // determine bricks removed from given wall
                    var bricksToRemove = realWall.Bricks
                        .Select(b => b.Id)
                        .Except(movedBricks)
                        .Except(wall.Bricks.Select(b => b.Id))
                        .ToList();
                    foreach (var bid in bricksToRemove)
                    {
                        db.Bricks.Remove(db.Bricks.FirstOrDefault(b => b.Id == bid));
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
                    db.Walls.Add(wall);
                }

                // remove walls
                foreach (var realWall in db.Walls.Where(w => w.OwnerId == ownerId).ToList())
                {
                    var ids = walls.Where(w => w.Id != 0).Select(w => w.Id).ToList();
                    if (!ids.Contains(realWall.Id))
                    {
                        foreach(var brick in realWall.Bricks.ToList())
                        {
                            realWall.Bricks.Remove(brick);
                        }

                        db.Walls.Remove(realWall);
                    }
                }
                */
                db.SaveChanges();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("DesignScene", scene);
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
            if (string.IsNullOrWhiteSpace(title))
            {
                ModelState.AddModelError("EmptyTemplate", "Template name cannot be empty");
            }

            if (ModelState.IsValid)
            {
                var template = new SceneTemplate();
                template.Id = Guid.NewGuid();
                template.Title = title;

                // TODO: Fix that
               // SceneTemplating.CloneScene(ownerId, template.Id, db);

                db.SceneTemplates.Add(template);

                db.SaveChanges();
            }

            return View("Empty");
        }

        // TODO: Fix that
        /*
        [HttpPost]
        public ActionResult ApplyTemplate(Guid ownerId, Guid templateId)
        {
            // delete obsolete scene
            foreach (var wall in db.Walls.Where(w => w.OwnerId == ownerId).ToList())
            {
                db.Walls.Remove(wall);
            }

            SceneTemplating.CloneScene(templateId, ownerId, db);

            db.SaveChanges();

            return PartialView("DesignScene", db.Walls.Where(w => w.OwnerId == ownerId));
        }
        */
        [HttpPost]
        public ActionResult CanDeleteBrick(Brick brick)
        {
            return new JsonResult() { Data = !db.Bricks.OfType<LinkableBrick>().Where(b => b.LinkedBrickId == brick.Id).Any() };
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}