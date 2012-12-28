using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Core;

namespace Bnh.Infrastructure
{
    public class ServerPathMapper : IPathMapper
    {
        public string Map(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}