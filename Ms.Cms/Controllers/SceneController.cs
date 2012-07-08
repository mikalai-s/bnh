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


        [HttpGet]
        public ActionResult EditLinkable()
        {
            return View(ContentUrl.Views.Scene.EditLinkable);
        }


        // POST: /Scene/Edit/5

        [HttpPost]
        public ActionResult Save(Scene scene)
        {
            if (ModelState.IsValid)
            {
                SaveScene(scene);
            }
            
            if (Request.IsAjaxRequest())
            {
                // render real (saved) scene
                return PartialView(ContentUrl.Views.Scene.Partial.DesignScene, db.Scenes.First(s => s.SceneId == scene.SceneId));
            }
            
            return View(ContentUrl.Views.Scene.Partial.DesignScene);
        }

        private void SaveScene(Scene scene, bool cloning = false)
        {
            // enumerate scene's existing brick contents to track deleted ones
            var existingSceneWalls = db.Scenes
                .Where(s => s.SceneId == scene.SceneId)
                .ToList() // because SelectMany() is not supported by MongoDB Linq yet
                .SelectMany(s => s.Walls);
            var existingBricks = existingSceneWalls
                .SelectMany(w => w.Bricks.Select(b => b.BrickContentId))
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            // TODO: it's good to implement some kind of version check on each scene
            // so that if designScene version is different from realScene version 
            // we throw exception saying that somebody has already updated scene 
            // while a user was editing it.

            foreach (var wall in scene.Walls)
            {
                foreach (var brick in wall.Bricks)
                {
                    if (cloning)
                    {
                        if (!string.IsNullOrEmpty(brick.BrickContentId)) // doesn't make sense to clone empty brick
                        {
                            var content = db.BrickContents.First(c => c.BrickContentId == brick.BrickContentId);
                            content.BrickContentId = null;
                            db.BrickContents.Insert(content);
                            brick.BrickContentId = content.BrickContentId;
                        }
                    }
                    else if (string.IsNullOrEmpty(brick.NewContentTypeName)) // existing brick
                    {
                        existingBricks.Remove(brick.BrickContentId);
                    }
                    else  // not empty brick
                    {
                        var contentType = MsCms.RegisteredBrickTypes
                            .Where(br => br.Type.Name == brick.NewContentTypeName)
                            .Select(br => br.Type)
                            .First();
                        var newContent = Activator.CreateInstance(contentType) as BrickContent;
                        db.BrickContents.Insert(newContent);
                        brick.BrickContentId = newContent.BrickContentId;
                    }
                }
            }

            // save scene document
            db.Scenes.Save(scene);

            // delete removed bricks
            existingBricks.ForEach(db.BrickContents.Delete);
        }

        [HttpPost]
        public ActionResult ApplyTemplate(string sceneId, string templateSceneId)
        {
            var template = db.Scenes.First(t => t.SceneId == templateSceneId && t.IsTemplate);
            template.SceneId = sceneId;
            template.Title = null;
            SaveScene(template, true);

            return PartialView(ContentUrl.Views.Scene.Partial.DesignScene, template);
        }

        [HttpPost]
        public ActionResult CanDeleteBrick(Brick brick)
        {
            if (brick == null || string.IsNullOrEmpty(brick.BrickContentId))
            {
                return new JsonResult { Data = true };
            }
            else
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
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}