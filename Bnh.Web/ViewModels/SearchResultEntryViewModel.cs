using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh.Web.ViewModels
{
    public class SearchResultEntryViewModel
    {
        public string Category { get; set; }

        public string Link { get; set; }

        public string[] Fragments { get; set; }
    }
}
