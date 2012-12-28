using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Web.ViewModels
{
    public class SearchViewModel
    {
        public string Query { get; set; }

        public IEnumerable<SearchResultEntryViewModel> Result { get; set; }

        public SearchViewModel()
        {
            Result = Enumerable.Empty<SearchResultEntryViewModel>();
        }
    }
}