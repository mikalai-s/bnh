using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.ViewModels
{
    public class SceneViewModel
    {
        public IEnumerable<WallViewModel> Walls { get; set; }

        public string TemplateTitle { get; set; }

        public IEnumerable<SelectListItem> Templates { get; set; }
    }
}