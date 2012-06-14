using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms.Models
{
    public static class SceneExtensions
    {
        public static Scene ApplyTemplate(this Scene scene, Scene templateScene)
        {
            // delete all walls on obsolete scene
            foreach (var wall in scene.Walls.ToList())
            {
                scene.Walls.Remove(wall);
            }

            // clone walls from template scene into our scene
            foreach (var wall in templateScene.Walls)
            {
                scene.Walls.Add(wall.Clone());
            }

            return scene;
        }
    }
}