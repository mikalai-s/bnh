using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Web.ViewModels
{
    public class FilterPropertyViewModel
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Comparer { get; set; }

        public object DefaultValue { get; set; }

        public Type Type { get; set; }
    }
}