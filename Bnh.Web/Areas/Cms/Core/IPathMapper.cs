using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Core
{
    public interface IPathMapper
    {
        string Map(string path);
    }
}