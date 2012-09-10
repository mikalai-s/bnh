using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Cms.ViewModels;

namespace Bnh.Cms.Models
{
    public static class SceneExtensions
    {
        public static SceneViewModel ToViewModel(this Scene scene, CmsEntities db)
        {
            return new SceneViewModel
            {
                SceneId = scene.SceneId,
                Title = scene.Title,
                IsTemplate = scene.IsTemplate,
                Walls = scene.Walls.Select(wall => new WallViewModel
                {
                    Title = wall.Title,
                    Width = wall.Width,
                    Bricks = wall.Bricks.Select(brick => new BrickViewModel
                    {
                        Title = brick.Title,
                        Width = brick.Width,
                        BrickContentId = brick.BrickContentId,
                        Content = brick.GetContent(db),
                    })
                })
            };
        }
    }
}