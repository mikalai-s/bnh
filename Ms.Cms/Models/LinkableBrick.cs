using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ms.Cms.Models
{
    public partial class LinkableBrick : Brick
    {
        public long? LinkedBrickId { get; set; }

        //[ForeignKey("LinkedBrickId")]
        public virtual Brick LinkedBrick { get; set; }
    }
    
}
