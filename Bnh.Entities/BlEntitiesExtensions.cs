using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ms.Cms.Models;

namespace Bnh.Entities
{
    public static class BlEntitiesExtensions
    {
        /// <summary>
        /// Get community's scene. If it doesn't exists it creates one.
        /// </summary>
        /// <param name="community">Community to get scene for</param>
        /// <returns></returns>
        public static Scene GetScene(this Community community)
        {
            using (var cmd = new CmsEntities())
            {
                var scene = cmd.Scenes.FirstOrDefault(s => s.Id == community.SceneId);
                if (scene == null)
                {
                    cmd.Scenes.Insert(scene);

                    using (var bl = new BleEntities())
                    {
                        community.SceneId = scene.Id;
                        bl.Communities.Save(community);
                    }
                }
                return scene;
            }
        }
    }
}