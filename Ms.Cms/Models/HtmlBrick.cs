using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ms.Cms.Models
{
    public partial class HtmlBrick : Brick
    {
        [DataMemberAttribute()]
        public string Html { get; set; }
    }
    
}
