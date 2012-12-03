using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Web.ViewModels
{
    public class SearchViewModel
    {
        public string Query { get; set; }

        public ICollection<SearchResultEntryViewModel> Result { get; set; }
    }
}