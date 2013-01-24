using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Bnh.Cms.Models
{
    public partial class HtmlContent : BrickContent
    {
        [AllowHtml]
        public string Html { get; set; }
    }
}
