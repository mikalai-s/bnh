using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms.Models
{
    public static class SceneExtensions
    {
        public static Scene ApplyTemplate(this Scene scene, CmsEntities db, Scene templateScene)
        {
            // delete all walls on obsolete scene
            foreach (var wall in scene.Walls.ToList())
            {
                //db.Walls.Delete(wall.Id);
            }

            // clone walls from template scene into our scene
            foreach (var wall in templateScene.Walls)
            {
                scene.Walls.Add(wall.Clone());
            }

            return scene;
        }

        /// <summary>
        /// Converts scene into design scene
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static DesignScene ToDesignScene(this Scene scene)
        {
            var designScene = new DesignScene
            {
                Id = scene.Id,
                IsTemplate = scene.IsTemplate,
                OwnerGuidId = scene.OwnerGuidId,
                Title = scene.Title
            };

            var walls = scene.Walls.ToList();
            foreach (var wall in walls)
            {
                var designWall = new DesignWall
                {
                    Title = wall.Title,
                    Width = wall.Width,
                    OriginalIndex = walls.IndexOf(wall)
                };

                var bricks = wall.Bricks.ToList();
                foreach (var brick in bricks)
                {
                    var designBrick = new DesignBrick
                    {
                        TypeName = brick.GetType().Name,
                        BrickContentId = brick.BrickContentId,
                        Title = brick.Title,
                        Width = brick.Width,
                        OriginalIndex = bricks.IndexOf(brick),
                        OriginalWallIndex = designWall.OriginalIndex
                    };
                    designWall.Bricks.Add(designBrick);
                };

                designScene.Walls.Add(designWall);
            }

            return designScene;
        }
    }
}