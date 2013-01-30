using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Core;
using Cms.ViewModels;

namespace Cms.Models
{
    public static class SceneExtensions
    {
        public static SceneViewModel ToViewModel(this Scene scene, ViewModelContext context)
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
                    Bricks = wall.Bricks.Select(brick => BrickViewModel<BrickContent>.Create(context, brick))
                })
            };
        }
    }
}