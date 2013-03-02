using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Helpers;
using Cms.Models;

namespace Cms.ViewModels
{
    public class RazorBrickViewModel : BrickViewModel<RazorBrick>
    {
        public MvcHtmlString Html
        {
            get
            {
                return new MvcHtmlString(RazorEngine.GetContent(this.Content.Html, this.Context.SceneHolder));
            }
        }

        public RazorBrickViewModel(SceneViewModelContext context, RazorBrick brick)
            : base (context, brick)
        {
        }
    }
}