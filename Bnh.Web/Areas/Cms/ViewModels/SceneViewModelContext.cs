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
        public Scene Scene { get; private set; }

        public SceneViewModelContext(Controller controller, IConfig config, IRepositories repos, IRatingCalculator rating, Scene scene)
            : base(controller, config, repos, rating)
        {
            this.Scene = scene;
        }
    }
}