using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Models;

namespace Cms.ViewModels
{
    public class LinkableBrickViewModel : BrickViewModel<LinkableBrick>
    {
        public IEnumerable<SelectListItem> BricksToLink
        {
            get
            {
                return this.Context.Repos.SpecialScenes
                    .Single(s => s.SceneId == Constants.LinkableBricksSceneId)
                    .Scene.Walls.SelectMany(w => w.Bricks)
                    .Select(b => new SelectListItem { Value = b.BrickId, Text = b.Title });
            }
        }

        public LinkableBrickViewModel(SceneViewModelContext context, LinkableBrick brick)
            : base (context, brick)
        {
        }

        // TODO: as an idea to avoid one more DB call for linkable contents - we can cache them
        public override object EnsureNonLinkable()
        {
            if (!this.Content.LinkedBrickId.IsNullOrEmpty() && this.Context.LinkableBricks.Value.ContainsKey(this.Content.LinkedBrickId))
            {
                return BrickViewModel<Brick>.Create(this.Context, this.Context.LinkableBricks.Value[this.Content.LinkedBrickId]);
            }

            return base.EnsureNonLinkable();
        }
    }
}