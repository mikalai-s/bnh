using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Core;
using Cms.Models;

namespace Cms.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneViewModelContext : ViewModelContext
    {
        public ISceneHolder SceneHolder { get; private set; }

        public Lazy<Dictionary<string, Brick>> LinkableBricks { get; private set; }


        public SceneViewModelContext(Controller controller, IConfig config, IRepositories repos, IRatingCalculator rating, ISceneHolder sceneHolder)
            : base(controller, config, repos, rating)
        {
            this.SceneHolder = sceneHolder;

            this.LinkableBricks = new Lazy<Dictionary<string,Brick>>(() =>
                this.Repos.SpecialScenes.Single(s => s.SceneId == Constants.LinkableBricksSceneId)
                    .Scene.Walls.SelectMany(w => w.Bricks)
                    .ToDictionary(b => b.BrickId, b => b));
        }

    }
}