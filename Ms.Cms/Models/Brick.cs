using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ms.Cms.Models
{
    [Table("Bricks", Schema = "Cms")]
    public abstract partial class Brick
    {
        public long Id { get; set; }
        
        [MaxLength(50)]
        public string Title { get; set; }

        public float Width { get; set; }

        public byte Order { get; set; }

        [MaxLength(50)]
        public string ContentTitle { get; set; }

        public virtual Wall Wall { get; set; }

        public long WallId { get; set; }
    }

}
