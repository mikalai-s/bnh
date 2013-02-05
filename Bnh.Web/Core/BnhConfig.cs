using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Cms.Core;

namespace Bnh.Core
{
    public class BnhConfig : IBnhConfig
    {
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public IDictionary<string, IDictionary<string, string>> Authentification { get; set; }
        public string City { get; set; }
        public ReviewConfig Review { get; set; }
        public string SearchIndexFolder { get; set; }
        public IDictionary<string, string> Roles { get; set; }
    }
}