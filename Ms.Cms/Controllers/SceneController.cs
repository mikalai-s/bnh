using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ms.Cms.Models;
using System.Configuration;

using MongoDB.Driver.Linq;
using MongoDB.Bson;


namespace Ms.Cms.Controllers
{
    [DesignerAuthorizeAttribute]
    public class SceneController : Controller
    {
        private CmsEntities db = new CmsEntities();


        // GET: /Scene/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.Templates = new SelectList(
                db.Scenes.Where(s => s.IsTemplate).Select(s => new { id = s.Id, title = s.Title }), 
                "id", 
                "title");

            ViewBag.OwnerId = id;
            ViewBag.LinkableBricksSceneId = Constants.LinkableBricksSceneId;

            return View("~/WebExtracted/Ms.Cms/Views/Scene/Edit.cshtml", db.Scenes.First(s => s.Id == id).ToDesignScene());
        }

        // POST: /Scene/Edit/5

        [HttpPost]
        public ActionResult Save(DesignScene scene)
        {
            if (ModelState.IsValid)
            {
                // get real scene document to update
                var existingScene = db.Scenes.First(s => s.Id == scene.Id);
                var newScene = new Scene
                {
                    Id = scene.Id,
                    Title = scene.Title,
                    OwnerGuidId = scene.OwnerGuidId
                };

                // enumerate scene's existing brick contents to track deleted ones
                var existingBricks = existingScene.Walls.SelectMany(w => w.Bricks.Select(b => b.BrickContentId)).ToList();

                // TODO: it's good to implement some kind of version check on each scene
                // so that if designScene version is different from realScene version 
                // we throw exception saying that somebody has already updated scene 
                // while a user was editing it.

                foreach(var designWall in scene.Walls.Cast<DesignWall>())
                {
                    // if design wall has OriginalIndex property set,
                    // then it exists, otherwise new wall
                    var newWall = /*designWall.OriginalIndex.HasValue ?
                        existingScene.Walls.ElementAt(designWall.OriginalIndex.Value) :*/
                        new Wall();
                    newWall.Title = designWall.Title;
                    newWall.Width = designWall.Width;

                    newScene.Walls.Add(newWall);

                    foreach (var designBrick in designWall.Bricks.Cast<DesignBrick>())
                    {
                        if (designBrick.OriginalIndex.HasValue)
                        {
                            var brick = existingScene
                                .Walls.ElementAt(designBrick.OriginalWallIndex.Value)
                                .Bricks.ElementAt(designBrick.OriginalIndex.Value);
                            brick.Width = designBrick.Width;
                            brick.Title = designBrick.Title;
                            newWall.Bricks.Add(brick);

                            existingBricks.Remove(brick.BrickContentId);
                        }
                        else
                        {
                            var brickType = MsCms.RegisteredBrickTypes.Where(br => br.Type.Name == designBrick.TypeName).Select(br => br.Type).First();
                            var newBrick = Activator.CreateInstance(brickType) as BrickContent;
                            db.BrickContents.Insert(newBrick);

                            var linkableBrick = new Brick
                            {
                                BrickContentId = newBrick.Id,
                                Title = designBrick.Title,
                                Width = designBrick.Width
                            };
                            newWall.Bricks.Add(linkableBrick);
                        }
                    }
                }

                // delete removed bricks
                existingBricks.ForEach(db.BrickContents.Delete);

                db.Scenes.Save(newScene);
            }
            
            if (Request.IsAjaxRequest())
            {
                // render real (saved) scene
                return PartialView("~/WebExtracted/Ms.Cms/Views/Scene/DesignScene.cshtml", db.Scenes.First(s => s.Id == scene.Id).ToDesignScene());
            }
            
            return View("~/WebExtracted/Ms.Cms/Views/Scene/DesignScene.cshtml");
        }

        //// apply only properties that can be changed on scene designer
        //private Brick ApplyCommonBrickValues(Brick brick)
        //{
        //    var realBrick = db.Bricks.First(b => b.Id == brick.Id);
        //    realBrick.Order = brick.Order;
        //    realBrick.Width = brick.Width;
        //    return realBrick;
        //}

        [HttpPost]
        public ActionResult ExportTemplate(string title, long sceneId)
        {/*
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
            */
            return View("~/WebExtracted/Ms.Cms/Views/Scene/Empty.cshtml");
        }
      
        [HttpPost]
        public ActionResult ApplyTemplate(long sceneId, long templateSceneId)
        {/*
            var scene = db.Scenes.First(s => s.Id == sceneId).ApplyTemplate(db, db.Scenes.First(s => s.Id == templateSceneId));
            db.SaveChanges();
            */
            return PartialView("~/WebExtracted/Ms.Cms/Views/Scene/DesignScene.cshtml");//, scene);
        }

        [HttpPost]
        public ActionResult CanDeleteBrick(Brick brick)
        {
            return new JsonResult()
            {
                // because OfType() extension is not implemented in current MongoDd driver
                // the query is a little bit weird
                Data = !db.BrickContents
                    .Where(b => b is LinkableContent)
                    .Select(b => b as LinkableContent)
                    .ToList()
                    .Any(b => b.LinkedContentId == brick.BrickContentId)
            };
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}