using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Cms.Models
{
    public partial class HtmlBrick : Brick
    {
        [AllowHtml]
        public string Html { get; set; }
    }
}
