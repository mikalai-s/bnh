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
        public ActionResult Save(Scene scene)
        {
            if (ModelState.IsValid)
            {
                SceneUtils.SaveScene(this.repos, scene);
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