using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Ms.Cms.Models.Attributes;

namespace Ms.Cms.Models
{
    public partial class HtmlContent : BrickContent
    {
        [NonJsExposable()]
        public string Html { get; set; }
    }
    
}
