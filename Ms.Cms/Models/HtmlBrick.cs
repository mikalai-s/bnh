using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Ms.Cms.Models.Attributes;

namespace Ms.Cms.Models
{
    public partial class HtmlBrick : Brick
    {
        [NonJsExposable()]
        public string Html { get; set; }
    }
    
}
