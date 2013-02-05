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
            // just a simple check whether there is need to initilize data
            if (repos.Scenes.Where(s => s.SceneId == Constants.LinkableBricksSceneId).Any()) { return; }
                
            repos.Scenes.Insert(new Scene
            {
                SceneId = Constants.LinkableBricksSceneId,
                Title = "Linkable Bricks Scene",
                Walls = new [] 
                {
                    new Wall
                    {
                        Title = "Wall",
                        Width = 100.0f
                    }
                }
            });

            // add index to brick content
            repos.Scenes.Collection.EnsureIndex("Walls", "Bricks", "BrickContentId");
        }
    }
}