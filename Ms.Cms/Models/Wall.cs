using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Ms.Cms.Models.Attributes;

namespace Ms.Cms.Models
{
    [Table("Walls", Schema = "Cms")]
    public partial class Wall
    {
        public Wall()
        {
            this.Bricks = new HashSet<Brick>();
        }

        public long Id { get; set; }

        public long SceneId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public float Width { get; set; }

        public byte Order { get; set; }

        // navigational

        [NonJsExposable]
        public virtual Scene Scene { get; set; }

        [NonJsExposable]
        public virtual ICollection<Brick> Bricks { get; private set; }
    }

}
