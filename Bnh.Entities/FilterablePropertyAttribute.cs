using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh.Entities
{
    public class FilterablePropertyAttribute : Attribute
    {
        public string Title { get; set; }
    }
}
