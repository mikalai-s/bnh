using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh.Core.Entities
{
    public class City
    {
        public string CityId { get; set; }
        
        public string Name { get; set; }

        public string UrlId { get; set; }

        public IEnumerable<string> Zones { get; set; }
    }
}
