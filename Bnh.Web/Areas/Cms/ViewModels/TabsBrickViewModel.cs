using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Models;

namespace Cms.ViewModels
{
    public class TabsBrickViewModel : BrickViewModel<TabsBrick>
    {
        public IEnumerable<object> Bricks
        {
            get
            {
                return this.Context.SceneHolder.Scene.Walls
                    .First(w => w.Bricks.Any(b => b.BrickId == this.Content.BrickId))
                    .Bricks.Where(b => b.BrickId != this.Content.BrickId)
                    .Select(b => new { name = b.Title, value = b.BrickId });
            }
        }

        public Dictionary<string, string[]> Tabs
        {
            get
            {
                return (this.Content.Tabs == null)
                    ? new Dictionary<string, string[]>()
                    : this.Content.Tabs;
            }
        }

        public TabsBrickViewModel(SceneViewModelContext context, TabsBrick brick)
            : base(context, brick)
        {

        }
    }
}