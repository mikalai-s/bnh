using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

using Ms.Cms.Models.Attributes;

namespace Ms.Cms.Models
{
    [Table("Bricks", Schema = "Cms")]
    public abstract partial class Brick
    {
        [NonCloneable]
        public long Id { get; set; }
        
        [MaxLength(50)]
        public string Title { get; set; }

        public float Width { get; set; }

        public byte Order { get; set; }

        [MaxLength(50)]
        public string ContentTitle { get; set; }

        [NonJsExposable]
        [NonCloneable]
        public virtual Wall Wall { get; set; }

        [NonCloneable]
        public long WallId { get; set; }
    }

}
