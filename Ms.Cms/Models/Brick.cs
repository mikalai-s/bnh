using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Ms.Cms.Models
{
    [Table("Bricks", Schema = "Cms")]
    public abstract partial class Brick
    {
        [DataMemberAttribute()]
        public long Id { get; set; }
        
        [MaxLength(50)]
        [DataMemberAttribute()]
        public string Title { get; set; }

        [DataMemberAttribute()]
        public float Width { get; set; }

        [DataMemberAttribute()]
        public byte Order { get; set; }

        [MaxLength(50)]
        [DataMemberAttribute()]
        public string ContentTitle { get; set; }

        public virtual Wall Wall { get; set; }

        public long WallId { get; set; }
    }

}
