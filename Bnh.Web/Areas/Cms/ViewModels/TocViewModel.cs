using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Models;
using Cms.ViewModels;
using Cms.Helpers;

namespace Cms.ViewModels
{
    public class TocViewModel : BrickViewModel<TocContent>
    {
        public IEnumerable<BrickContent> TocBricks { get; private set; }

        public TocViewModel(ViewModelContext context, string title, float width, string brickContentId, TocContent content)
            : base(context, title, width, brickContentId, content)
        {
            var sceneContext = context as SceneViewModelContext;
            if (sceneContext == null)
            {
                this.TocBricks = Enumerable.Empty<TocContent>();
                return;
            }

            var bricks = sceneContext.Scene.Walls
                .SelectMany(w => w.Bricks)
                .Select(b => b.BrickContentId)
                .ToList();
            this.TocBricks = context.Repos.BrickContents
                .Where(c => bricks.Contains(c.BrickContentId))
                .Where(c => c.IsTitleUsedInToC)
                .Where(c => !string.IsNullOrEmpty(c.ContentTitle))
                .ToList()
                .Where(c => c.GetType() != typeof(TocContent))
                .OrderBy(c => bricks.IndexOf(c.BrickContentId))
                .ToList();
        }
    }
}