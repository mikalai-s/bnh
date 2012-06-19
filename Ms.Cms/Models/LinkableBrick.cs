using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Ms.Cms.Models.Attributes;

namespace Ms.Cms.Models
{
    public partial class LinkableBrick : Brick
    {
        public long? LinkedBrickId { get; set; }

        [NonJsExposable]
        public virtual Brick LinkedBrick { get; set; }
    }
    
}
