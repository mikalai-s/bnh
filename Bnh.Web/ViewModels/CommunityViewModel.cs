using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core.Entities;

namespace Bnh.Web.ViewModels
{
    public class CommunityViewModel
    {
        public string DeleteUrl { get; private set; }
        public string DetailsUrl { get; private set; }
        public string ReviewsUrl { get; private set; }

        public string Name { get; private set; }

        public CommunityViewModel(ViewModelContext context, Community community)
        {
            this.DeleteUrl = context.UrlHelper.Action("Delete", "Community", new { id = community.UrlId });
            this.DetailsUrl = context.UrlHelper.Action("Details", "Community", new { id = community.UrlId });
            this.ReviewsUrl = context.UrlHelper.Action("Reviews", "Reviews", new { id = community.UrlId });
            this.Name = community.Name;
        }
    }
}