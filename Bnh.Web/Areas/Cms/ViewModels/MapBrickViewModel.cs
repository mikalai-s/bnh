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
    public class MapBrickViewModel : BrickViewModel<MapBrick>
    {
        public MvcHtmlString GpsLocation { get; private set; }

        public MvcHtmlString GpsBounds { get; private set; }

        public MapBrickViewModel(SceneViewModelContext context, MapBrick brick)
            : base(context, brick)
        {
            if (context == null) { return; }

            var mappable = context.SceneHolder as IMappable;
            if (mappable == null)
            {
                this.GpsLocation = new MvcHtmlString(brick.GpsLocation);
                return;
            }

            this.GpsLocation = new MvcHtmlString(mappable.GpsLocation);
            this.GpsBounds = new MvcHtmlString(mappable.GpsBounds);
        }
    }
}