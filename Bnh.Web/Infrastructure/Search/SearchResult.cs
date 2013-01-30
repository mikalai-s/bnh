using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Core;
using Cms.Core;

namespace Bnh.Infrastructure.Search
{
    public class CommunitySearchResult : ISearchResult
    {
        public string CommunityId { get; set; }
        public string ContentId { get; set; }
        public string[] Fragments { get; set; }
    }
}
