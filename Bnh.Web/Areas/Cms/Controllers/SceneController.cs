using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;

using MongoDB.Driver.Linq;
using MongoDB.Bson;
using Cms.ViewModels;
using Cms.Core;
using Cms.Models;
using Cms.Infrastructure;
using Cms.Helpers;


namespace Cms.Controllers
{
    public class SceneController : Controller
    {
        private IConfig config;
        private IRepositories repos;
        private IRatingCalculator rating;

        public SceneController(IConfig config, IRepositories repos, IRatingCalculator rating)
        {
            this.config = config;
            this.repos = repos;
            this.rating = rating;
        }


        [HttpGet]
        public ActionResult EditLinkable()
        {
            return View(ContentUrl.Views.Scene.EditLinkable);
        }


        // POST: /Scene/Edit/5

        [DesignerAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(Scene scene)
        {
            if (ModelState.IsValid)
            {
                SaveScene(scene);
            }

            this.ViewBag.TocBricks = Enumerable.Empty<BrickContent>();

            if (Request.IsAjaxRequest())
            {
                // render real (saved) scene
                return PartialView(ContentUrl.Views.Scene.Partial.DesignScene, repos.Scenes.First(s => s.SceneId == scene.SceneId).ToViewModel(GetViewModelContext()));
            }

            return View(ContentUrl.Views.Scene.Partial.DesignScene);
        }

        private ViewModelContext GetViewModelContext()
        {
            return new ViewModelContext(this, this.config, this.repos, this.rating);
        }

        private void SaveScene(Scene scene, bool cloning = false)
        {
            // enumerate scene's existing brick contents to track deleted ones
            var existingSceneWalls = repos.Scenes
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
                            var content = repos.BrickContents.First(c => c.BrickContentId == brick.BrickContentId);
                            content.BrickContentId = null;
                            repos.BrickContents.Insert(content);
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
                        repos.BrickContents.Insert(newContent);
                        brick.BrickContentId = newContent.BrickContentId;
                    }
                }
            }

            // save scene document
            repos.Scenes.Save(scene);

            // delete removed bricks
            existingBricks.ForEach(repos.BrickContents.Delete);
        }

        [DesignerAuthorizeAttribute]
        [HttpPost]
        public ActionResult ApplyTemplate(string sceneId, string templateSceneId)
        {
            var template = repos.Scenes.First(t => t.SceneId == templateSceneId && t.IsTemplate);
            template.SceneId = sceneId;
            template.Title = null;
            template.IsTemplate = false;
            SaveScene(template, true);

            return PartialView(ContentUrl.Views.Scene.Partial.DesignScene, template.ToViewModel(GetViewModelContext()));
        }

        [DesignerAuthorizeAttribute]
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
                    Data = !repos.BrickContents
                        .Where(b => b is LinkableContent)
                        .Select(b => b as LinkableContent)
                        .ToList()
                        .Any(b => b.LinkedContentId == brick.BrickContentId)
                };
            }
        }

        public ActionResult Details(string sceneId, object model)
        {
            var scene = this.repos.Scenes.FirstOrDefault(s => s.SceneId == sceneId) ?? new Scene { SceneId = sceneId };

            var bricks = scene.Walls
                .SelectMany(w => w.Bricks)
                .Select(b => b.BrickContentId)
                .ToList();
            var tocContents = this.repos.BrickContents
                .Where(c => bricks.Contains(c.BrickContentId))
                .Where(c => c.IsTitleUsedInToC)
                .Where(c => !string.IsNullOrEmpty(c.ContentTitle))
                .ToList()
                .Where(c => c.GetType() != typeof(TocContent))
                .OrderBy(c => bricks.IndexOf(c.BrickContentId))
                .ToList();

            this.ViewBag.GlobalModel = model;
            this.ViewBag.TocBricks = tocContents;

            return PartialView(ContentUrl.Views.Scene.View, scene.ToViewModel(GetViewModelContext()));
        }

        [DesignerAuthorizeAttribute]
        public ActionResult Edit(string sceneId, object model)
        {
            var scene = this.repos.Scenes.FirstOrDefault(s => s.SceneId == sceneId) ?? new Scene { SceneId = sceneId };
            var templates = repos.Scenes
                .Where(s => s.IsTemplate && s.SceneId != scene.SceneId)
                .Select(s => new { id = s.SceneId, title = s.Title })
                .ToList();
            this.ViewBag.GlobalModel = model;
            this.ViewBag.Templates = new SelectList(templates, "id", "title");
            this.ViewBag.LinkableBricksSceneId = Constants.LinkableBricksSceneId;

            return PartialView(ContentUrl.Views.Scene.Edit, scene.ToViewModel(GetViewModelContext()));
        }
    }
}