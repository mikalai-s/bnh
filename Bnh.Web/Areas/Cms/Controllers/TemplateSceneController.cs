using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Core;
using Cms.Helpers;
using Cms.Infrastructure;
using Cms.Models;

namespace Cms.Controllers
{
    [DesignerAuthorize]
    public class TemplateSceneController : SceneController
    {
        IRepositories repos;

        public TemplateSceneController(IConfig config, IRepositories repos, IRatingCalculator rating)
            : base (config, repos, rating)
        {
            this.repos = repos;
        }

        protected override ISceneHolder GetSceneHolder(string templateId)
        {
            return this.repos.SpecialScenes.Single(h => h.SceneId == templateId);
        }

        protected override void SaveScene(string templateId, Scene scene)
        {
            var holder = this.repos.SpecialScenes.FirstOrDefault(h => h.SceneId == templateId);
            if (holder == null)
            {
                holder = new SpecialScene();
            }
            holder.Scene = scene;
            this.repos.SpecialScenes.Save(holder);
        }


        public ActionResult Create(string title, string sceneJson)
        {
            if (this.repos.SpecialScenes.Any(s => s.Title == title))
            {
                return Content("Template with title \"{0}\" already exists".FormatWith(title));
            }

            var template = new SpecialScene
            {
                Title = title
            };
            this.repos.SpecialScenes.Save(template);

            return RedirectToAction("SaveScene", new { id = template.SceneId, sceneJson });
        }


        public ActionResult EditScene(string id)
        {
            return View(ContentUrl.Views.Scene.EditTemplate, (string)null, GetSceneHolder(id));
        }


        public ActionResult Index()
        {
            return View(ContentUrl.Views.Scene.TemplateIndex, this.repos.SpecialScenes.Where(s => s.SceneId != Constants.LinkableBricksSceneId));
        }
    }
}