using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh.ViewModels
{
    public class SearchResultEntryViewModel
    {
        public string Category { get; set; }

        public string Link { get; set; }

        public string[] Fragments { get; set; }
    }
}
