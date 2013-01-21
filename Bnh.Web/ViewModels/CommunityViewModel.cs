using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Bnh.Core;
using Bnh.Core.Entities;

namespace Bnh.Web.ViewModels
{
    public class CommunityViewModel
    {
        public string DeleteUrl { get; private set; }

        public string DetailsUrl { get; private set; }

        public string ReviewsUrl { get; private set; }

        public string Name { get; private set; }

        public string UrlId { get; private set; }

        public string GpsBounds { get; private set; }

        public string GpsLocation { get; private set; }

        public IDictionary<string, object> Properties { get; private set; }

        public string PopupHtml { get; private set; }

        public CommunityViewModel(ViewModelContext context, Community community, IRatingCalculator ratingCalculator)
        {
            this.DeleteUrl = context.UrlHelper.Action("Delete", "Community", new { id = community.UrlId });
            this.DetailsUrl = context.UrlHelper.Action("Details", "Community", new { id = community.UrlId });
            this.ReviewsUrl = context.UrlHelper.Action("Reviews", "Community", new { id = community.UrlId });
            this.Name = community.Name;
            this.UrlId = community.UrlId;
            this.GpsBounds = community.GpsBounds;
            this.GpsLocation = community.GpsLocation;

            this.Properties = FilterProperty.Get(typeof(Community))
                .ToDictionary(
                    p => p.Property.Name,
                    p => p.Property.GetValue(community, null)
                );

            this.PopupHtml = "<div><a href='{1}'>{2}</a></div><div>{0}</div><div><a href='{3}'>{4}</a></div>".FormatWith(
                context.HtmlHelper.RatingStars(ratingCalculator.GetTargetRating(community.CommunityId)),
                this.DetailsUrl, this.Name,
                this.ReviewsUrl, "Reviews");
        }
    }
}