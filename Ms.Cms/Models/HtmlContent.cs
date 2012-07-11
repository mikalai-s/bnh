using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Ms.Cms.Models.Attributes;

namespace Ms.Cms.Models
{
    public partial class HtmlContent : BrickContent
    {
        [NonJsExposable()]
        [AllowHtml]
        public string Html { get; set; }
    }
    
}
