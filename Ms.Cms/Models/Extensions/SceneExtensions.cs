using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms.Models
{
    public static class SceneExtensions
    {
        /// <summary>
        /// Converts scene into design scene
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static Scene ToDesignScene(this Scene scene)
        {
            var l = scene.Walls.SelectMany((w, i) => 
                w.Bricks.Select((b, j) =>
                {
                    b.OriginalIndex = j;
                    b.OriginalWallIndex = i;
                    return b;
                }).ToList()
            ).ToList();
            return scene;
        }
    }
}