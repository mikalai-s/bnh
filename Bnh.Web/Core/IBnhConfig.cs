using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cms.Core;

namespace Bnh.Core
{
    public interface IBnhConfig : IConfig
    {
        string City { get; set; }

        bool IsValidHost(HttpContextBase request);

        string UploadsFolder { get; set; }
    }
}