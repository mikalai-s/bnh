using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Models
{
    public interface ISceneHolder
    {
        Scene Scene { get; }
    }
}