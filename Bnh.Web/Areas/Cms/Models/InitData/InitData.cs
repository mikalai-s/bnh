using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Core;

namespace Cms.Models
{
    internal static class InitData
    {
        public static void Init(IRepositories repos)
        {
            // just a simple check whether there is need to initialize data
            if (repos.SpecialScenes.Where(s => s.SceneId == Constants.LinkableBricksSceneId).Any()) { return; }

            repos.SpecialScenes.Insert(new SpecialScene
            {
                SceneId = Constants.LinkableBricksSceneId,
                Scene = new Scene
                {
                    Title = "Linkable Bricks Scene",
                    Walls = new[] 
                    {
                        new Wall
                        {
                            Title = "Wall",
                            Width = 100.0f
                        }
                    }
                }
            });
        }
    }
}