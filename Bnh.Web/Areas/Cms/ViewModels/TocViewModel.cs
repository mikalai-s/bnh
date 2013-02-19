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
        public IEnumerable<LinkViewModel> Links { get; private set; }

        public TocViewModel(ViewModelContext context, string title, float width, string brickContentId, TocContent content)
            : base(context, title, width, brickContentId, content)
        {
            //var scene = context.Repos.Scenes.Where(s => s.content.GetSceneId()
            //var bricks = scene.Walls
            //    .SelectMany(w => w.Bricks)
            //    .Select(b => b.BrickContentId)
            //    .ToList();
            //var tocContents = this.repos.BrickContents
            //    .Where(c => bricks.Contains(c.BrickContentId))
            //    .Where(c => c.IsTitleUsedInToC)
            //    .Where(c => !string.IsNullOrEmpty(c.ContentTitle))
            //    .ToList()
            //    .Where(c => c.GetType() != typeof(TocContent))
            //    .OrderBy(c => bricks.IndexOf(c.BrickContentId))
            //    .ToList();
        }
    }
}