using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Core.Entities;

namespace Bnh.Web.ViewModels
{
    public class CommunityViewModel
    {
        public Community Community { get; set; }

        public string DeleteUrl { get; set; }

        public string DetailsUrl { get; set; }

        public string InfoPopupHtml { get; set; }
    }
}