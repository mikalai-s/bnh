using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;


using Cms.ViewModels;
using Cms.Core;
using Cms.Models;
using Cms.Infrastructure;
using Cms.Helpers;
using Cms.Utils;


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
        public ActionResult Save(SceneViewModel scene)
        {
            if (ModelState.IsValid)
            {
                var originalSceneBricks = this.repos.Scenes
                    .Where(s => s.SceneId == scene.SceneId)
                    .ToList()
                    .SelectMany(s => s.Walls)
                    .SelectMany(w => w.Bricks)
                    .ToDictionary(
                        b => b.BrickId,
                        b => b);

                var sceneEntity = new Scene();
                sceneEntity.SceneId = scene.SceneId;
                sceneEntity.Title = scene.Title;
                sceneEntity.IsTemplate = scene.IsTemplate;

                var wallList = new List<Wall>();

                foreach (var wall in scene.Walls)
                {
                    var wallEntity = new Wall();
                    wallEntity.Title = wall.Title;
                    wallEntity.Width = wall.Width;

                    wallList.Add(wallEntity);

                    var brickList = new List<Brick>();

                    foreach (var brick in wall.Bricks)
                    {
                        Brick brickEntity = null;
                        if (!brick.BrickId.IsEmpty() && originalSceneBricks.ContainsKey(brick.BrickId))
                        {
                            brickEntity = originalSceneBricks[brick.BrickId];
                        }

                        if (brickEntity == null)
                        {
                            var brickType = brick.GetType().GetGenericArguments()[0];
                            brickEntity = (Brick)Activator.CreateInstance(MsCms.RegisteredBrickTypes.Single(b => b.Type == brickType).Type);
                            brickEntity.BrickId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
                        }

                        brickEntity.Title = brick.Title;
                        brickEntity.Width = brick.Width;

                        brickList.Add(brickEntity);
                    }

                    wallEntity.Bricks = brickList;
                }

                sceneEntity.Walls = wallList;

                this.repos.Scenes.Save(sceneEntity);
            }

            if (Request.IsAjaxRequest())
            {
                // render real (saved) scene
                var sc = repos.Scenes.First(s => s.SceneId == scene.SceneId);
                return PartialView(ContentUrl.Views.Scene.Partial.DesignScene, sc.ToViewModel(GetSceneViewModelContext(sc)));
            }

            return View(ContentUrl.Views.Scene.Partial.DesignScene);
        }

        private ViewModelContext GetViewModelContext()
        {
            return new ViewModelContext(this, this.config, this.repos, this.rating);
        }

        private ViewModelContext GetSceneViewModelContext(Scene scene)
        {
            return new SceneViewModelContext(this, this.config, this.repos, this.rating, scene);
        }

        

        [DesignerAuthorizeAttribute]
        [HttpPost]
        public ActionResult ApplyTemplate(string sceneId, string templateSceneId)
        {
            var template = SceneUtils.ApplyTemplate(this.repos, templateSceneId, sceneId);

            return PartialView(ContentUrl.Views.Scene.Partial.DesignScene, template.ToViewModel(GetSceneViewModelContext(template)));
        }

        [DesignerAuthorizeAttribute]
        [HttpPost]
        public ActionResult CanDeleteBrick(Brick brick)
        {
            if (brick == null || string.IsNullOrEmpty(brick.BrickId))
            {
                return new JsonResult { Data = true };
            }
            else
            {
                return new JsonResult()
                {
                    // because OfType() extension is not implemented in current MongoDd driver
                    // the query is a little bit weird
                    Data = repos.Scenes.CanDeleteBrick(brick.BrickId)
                };
            }
        }

        public ActionResult Details(string sceneId, object model)
        {
            var scene = this.repos.Scenes.FirstOrDefault(s => s.SceneId == sceneId) ?? new Scene { SceneId = sceneId };

            this.ViewBag.GlobalModel = model;

            return PartialView(ContentUrl.Views.Scene.View, scene.ToViewModel(GetSceneViewModelContext(scene)));
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

            return PartialView(ContentUrl.Views.Scene.Edit, scene.ToViewModel(GetSceneViewModelContext(scene)));
        }
    }
}