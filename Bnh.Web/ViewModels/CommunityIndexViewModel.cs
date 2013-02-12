using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.ViewModels;
using Bnh.Core;
using Bnh.Core.Entities;
using Cms.Core;

namespace Bnh.ViewModels
{
    public class CommunityIndexViewModel : PageViewModel
    {
        public IDictionary<string, IEnumerable<CommunityViewModel>> Zones { get; set; }

        public CommunityIndexViewModel(ViewModelContext context, IEnumerable<string> zones, IEnumerable<Community> communities)
        {
            var grouped = from zone in zones
                          join community in communities on zone equals community.Zone into groups
                          select new { zone, groups };

            this.Zones = grouped.ToDictionary(
                e => e.zone,
                e => e.groups.OrderBy(c => c.Name).Select(c => new CommunityViewModel(context, c)));
        }
    }
}