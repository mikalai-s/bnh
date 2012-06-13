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
            return View(scene);
        }

        // POST: /Scene/Edit/5

        [HttpPost]
        public ActionResult Save(Scene scene)
        {
            if (ModelState.IsValid)
            {
                // ensure walls collection is not null
                var walls = scene.Walls ?? new List<Wall>();

                // get moved bricks
                var movedBricks = (from wall in walls
                                   from brick in wall.Bricks
                                   where wall.Id != brick.WallId && brick.Id != 0
                                   select brick.Id).ToList();

                // update existing walls
                foreach (var wall in walls.Where(w => w.Id != 0))
                {
                    var realWall = db.Walls.First(w => w.Id == wall.Id);
                    realWall.Order = wall.Order;
                    realWall.Width = wall.Width;

                    // determine bricks removed from given wall
                    var bricksToRemove = realWall.Bricks
                        .Select(b => b.Id)
                        .Except(movedBricks)
                        .Except(wall.Bricks.Select(b => b.Id))
                        .ToList();
                    foreach (var bid in bricksToRemove)
                    {
                        db.Bricks.Remove(db.Bricks.First(b => b.Id == bid));
                    }

                    // add new bricks 
                    foreach (var brick in wall.Bricks.Where(b => b.Id == 0).ToList())
                    {
                        realWall.Bricks.Add(brick);
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
                        realWall.Bricks.Add(realBrick);
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
                        wall.Bricks.Add(realBrick);
                    }

                    wall.SceneId = scene.Id;
                    db.Walls.Add(wall);
                }

                // remove walls
                foreach (var realWall in db.Walls.Where(w => w.SceneId == scene.Id).ToList())
                {
                    var ids = walls.Where(w => w.Id != 0).Select(w => w.Id).ToList();
                    if (!ids.Contains(realWall.Id))
                    {
                        foreach(var brick in realWall.Bricks.ToList())
                        {
                            db.Bricks.Remove(brick);
                        }

                        db.Walls.Remove(realWall);
                    }
                }

                db.SaveChanges();
            }

            if (Request.IsAjaxRequest())
            {
                // render real (saved) scene
                return PartialView("DesignScene", db.Scenes.First(s => s.Id == scene.Id));
            }

            return View("DesignScene");
        }

        // apply only properties that can be changed on scene designer
        // TODO: maybe disable validation? http://stackoverflow.com/a/5573119/444630
        private Brick ApplyCommonBrickValues(Brick brick)
        {
            var realBrick = db.Bricks.First(b => b.Id == brick.Id);
            realBrick.Order = brick.Order;
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