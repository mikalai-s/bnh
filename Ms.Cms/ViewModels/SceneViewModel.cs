using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ms.Cms.ViewModels
{
    public class SceneViewModel
    {
        public string SceneId { get; set; }
        public IEnumerable<WallViewModel> Walls { get; set; }
        public bool IsTemplate { get; set; }
        public string Title { get; set; }
    }
}