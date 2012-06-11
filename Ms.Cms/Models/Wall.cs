using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Ms.Cms.Models
{
    [Table("Walls", Schema = "Cms")]
    public partial class Wall
    {
        public Wall()
        {
            this.Bricks = new HashSet<Brick>();
        }

        [DataMemberAttribute()]
        public long Id { get; set; }

        public long SceneId { get; set; }

        [MaxLength(50)]
        [DataMemberAttribute()]
        public string Title { get; set; }

        [DataMemberAttribute()]
        public float Width { get; set; }

        [DataMemberAttribute()]
        public byte Order { get; set; }

        // navigational

        public virtual Scene Scene { get; set; }

        public virtual ICollection<Brick> Bricks { get; private set; }
    }

}
