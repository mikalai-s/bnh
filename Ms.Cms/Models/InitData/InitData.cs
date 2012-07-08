using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms.Models
{
    internal static class InitData
    {
        public static void Init()
        {
            using (var db = new CmsEntities())
            {
                // just a simple check whether there is need to initilize data
                if (db.Scenes.Where(s => s.SceneId == Constants.LinkableBricksSceneId).Any()) { return; }
                
                db.Scenes.Insert(new Scene
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
            }
        }
    }
}