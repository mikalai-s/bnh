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
    public class LinkablesSceneController : SceneController
    {
        IRepositories repos;

        public LinkablesSceneController(IConfig config, IRepositories repos, IRatingCalculator rating)
            : base (config, repos, rating)
        {
            this.repos = repos;
        }

        protected override ISceneHolder GetSceneHolder(string entityId)
        {
            return this.repos.SpecialScenes.Single(h => h.SceneId == Constants.LinkableBricksSceneId);
        }

        protected override void SaveScene(string entityId, Scene scene)
        {
            var holder = this.repos.SpecialScenes.Single(h => h.SceneId == Constants.LinkableBricksSceneId);
            holder.Scene = scene;
            this.repos.SpecialScenes.Save(holder);
        }


        public ActionResult EditScene()
        {
            return View(ContentUrl.Views.Scene.EditLinkable, (string)null, GetSceneHolder(null));
        }
    }
}