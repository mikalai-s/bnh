using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Cms.Core
{
    public interface IConfig
    {
        IDictionary<string, string> ConnectionStrings { get; set; }
        IDictionary<string, IDictionary<string, string>> Authentification { get; set; }
        ReviewConfig Review { get; set; }
        string SearchIndexFolder { get; set; }
        IDictionary<string, string> Roles { get; set; }
    }

    public class ReviewConfig
    {
        public IDictionary<string, string> Questions { get; set; }
    }
}