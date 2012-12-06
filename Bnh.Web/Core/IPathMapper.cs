using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Core
{
    public interface IPathMapper
    {
        string Map(string path);
    }
}