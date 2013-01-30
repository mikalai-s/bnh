using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bnh.ViewModels
{
    public abstract class PageViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}