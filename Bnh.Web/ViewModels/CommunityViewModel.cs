using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Cms.ViewModels;
using Bnh.Core.Entities;
using Cms.Core;
using Cms.Helpers;

namespace Bnh.ViewModels
{
    public class CommunityViewModel
    {
        public string DeleteUrl { get; private set; }

        public string DetailsUrl { get; private set; }

        public string Name { get; private set; }

        public string UrlId { get; private set; }

        public string GpsBounds { get; private set; }

        public string GpsLocation { get; private set; }

        public IDictionary<string, object> Properties { get; private set; }

        public string PopupHtml { get; private set; }

        public CommunityViewModel(ViewModelContext context, Community community, double? rating)
        {
            this.DeleteUrl = context.UrlHelper.Action("Delete", "Community", new { id = community.UrlId });
            this.DetailsUrl = context.UrlHelper.Action("Details", "Community", new { id = community.UrlId });
            this.Name = community.Name;
            this.UrlId = community.UrlId;
            this.GpsBounds = community.GpsBounds;
            this.GpsLocation = community.GpsLocation;

            this.Properties = FilterProperty.Get(typeof(Community))
                .ToDictionary(
                    p => p.Property.Name,
                    p => p.Property.GetValue(community, null)
                );

            this.PopupHtml = "<div><a href='{1}'>{2}</a></div><div>{0}</div>{3}".FormatWith(
                context.Config.Review.RatingEnabled
                    ? context.HtmlHelper.RatingStars(rating)
                    : MvcHtmlString.Empty,
                this.DetailsUrl, this.Name, community.ShortDescription);
        }
    }
}