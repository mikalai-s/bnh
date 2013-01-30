using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Core
{
    public interface ISearchProvider
    {
        void RebuildIndex();

        IEnumerable<ISearchResult> Search(string query);
    }
}