using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Core;
using Cms.Models;

namespace Cms.Utils
{
    public class SceneUtils
    {
        public static Scene ApplyTemplate(IRepositories repos, string sourceSceneId, string targetSceneId)
        {
            var template = repos.Scenes.First(t => t.SceneId == sourceSceneId && t.IsTemplate);
            template.SceneId = targetSceneId;
            template.Title = null;
            template.IsTemplate = false;

            SaveScene(repos, template, true);

            return template;
        }

        public static void SaveScene(IRepositories repos, Scene scene)
        {
            SaveScene(repos, scene, false);
        }

        private static void SaveScene(IRepositories repos, Scene scene, bool cloning)
        {
            throw new NotImplementedException();
            /*
            // enumerate scene's existing brick contents to track deleted ones
            var existingSceneWalls = repos.Scenes
                .Where(s => s.SceneId == scene.SceneId)
                .ToList() // because SelectMany() is not supported by MongoDB Linq yet
                .SelectMany(s => s.Walls);
            var existingBricks = existingSceneWalls
                .SelectMany(w => w.Bricks.Select(b => b.BrickContentId))
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            // TODO: it's good to implement some kind of version check on each scene
            // so that if designScene version is different from realScene version 
            // we throw exception saying that somebody has already updated scene 
            // while a user was editing it.

            foreach (var wall in scene.Walls)
            {
                foreach (var brick in wall.Bricks)
                {
                    if (cloning)
                    {
                        if (!string.IsNullOrEmpty(brick.BrickContentId)) // doesn't make sense to clone empty brick
                        {
                            var content = repos.BrickContents.First(c => c.BrickContentId == brick.BrickContentId);
                            content.BrickContentId = null;
                            repos.BrickContents.Insert(content);
                            brick.BrickContentId = content.BrickContentId;
                        }
                    }
                    else if (string.IsNullOrEmpty(brick.NewContentTypeName)) // existing brick
                    {
                        existingBricks.Remove(brick.BrickContentId);
                    }
                    else  // not empty brick
                    {
                        var contentType = MsCms.RegisteredBrickTypes
                            .Where(br => br.Type.Name == brick.NewContentTypeName)
                            .Select(br => br.Type)
                            .First();
                        var newContent = Activator.CreateInstance(contentType) as Brick;
                        repos.BrickContents.Insert(newContent);
                        brick.BrickContentId = newContent.BrickContentId;
                    }
                }
            }

            // save scene document
            repos.Scenes.Save(scene);

            // delete removed bricks
            existingBricks.ForEach(repos.BrickContents.Delete);
          * */
        }
    }
}