using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ms.Cms.Models;

namespace Bnh.Entities
{
    public static class BlEntitiesExtensions
    {
        public static IEnumerable<Wall> GetWallsFromEntityId(Guid id)
        {
            using (var db = new CmsEntities())
            {
                // Convert to list to make sure context is open during request
                var walls = db.Scenes
                    .Where(s => s.OwnerGuidId == id)
                    .SelectMany(s => s.Walls)
                    .ToList();
                // ensure bricks while given db context is open
                walls.ForEach(w => w.Bricks.ToList());
                return walls;
            }
        }

        /// <summary>
        /// Get community's scene. If it doesn't exists it creates one.
        /// </summary>
        /// <param name="community">Community to get scene for</param>
        /// <returns></returns>
        public static Scene GetScene(this Community community)
        {
            using (var db = new CmsEntities())
            {
                var scene = db.Scenes.FirstOrDefault(s => s.OwnerGuidId == community.Id);
                if (scene == null)
                {
                    scene = new Scene { OwnerGuidId = community.Id };
                    db.Scenes.Insert(scene);
                }
                return scene;
            }
        }
    }
}