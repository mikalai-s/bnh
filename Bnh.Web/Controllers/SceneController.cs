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
            var sceneTemplates = from s in db.Scenes
                                 from t in db.SceneTemplates
                                 where s.OwnerGuidId == t.Id
                                 select new { id = s.Id, title = t.Title };
            ViewBag.Templates = new SelectList(sceneTemplates, "id", "title");

            ViewBag.OwnerId = id;

            var scene = db.Scenes.FirstOrDefault(s => s.Id == id); 
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
        private Brick ApplyCommonBrickValues(Brick brick)
        {
            var realBrick = db.Bricks.First(b => b.Id == brick.Id);
            realBrick.Order = brick.Order;
            realBrick.Width = brick.Width;
            return realBrick;
        }

        [HttpPost]
        public ActionResult ExportTemplate(string title, long sceneId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                ModelState.AddModelError("EmptyTemplate", "Template name cannot be empty");
            }

            if (ModelState.IsValid)
            {
                // create scene template first
                var template = new SceneTemplate { Id = Guid.NewGuid(), Title = title };
                db.SceneTemplates.Add(template);

                // find source scene and clone it
                var scene = db.Scenes.First(s => s.Id == sceneId).Clone();

                // update owner id of cloned scene
                scene.OwnerGuidId = template.Id;

                // add new scene to context and save it
                db.Scenes.Add(scene);
                db.SaveChanges();
            }

            return View("Empty");
        }

        // TODO: Fix that
        
        [HttpPost]
        public ActionResult ApplyTemplate(long sceneId, long templateSceneId)
        {
            var scene = db.Scenes.First(s => s.Id == sceneId);

            // delete all walls on obsolete scene
            foreach (var wall in scene.Walls.ToList())
            {
                db.Walls.Remove(wall);
            }

            // find template scene and clone it
            var templateScene = db.Scenes.First(s => s.Id == templateSceneId);

            // clone walls from template scene into our scene
            foreach (var wall in templateScene.Walls)
            {
                scene.Walls.Add(wall.Clone());
            }

            // save everything it
            db.SaveChanges();

            return PartialView("DesignScene", scene);
        }

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