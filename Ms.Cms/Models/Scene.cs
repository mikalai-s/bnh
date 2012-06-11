using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ms.Cms.Models
{
    [Table("Scene", Schema = "Cms")]
    public class Scene
    {
        public Scene()
        {
            this.Walls = new HashSet<Wall>();
        }

        public long Id { get; set; }

        public Guid OwnerGuidId { get; set; }

        public long OwnerLongId { get; set; }

        public int OwnerIntId { get; set; }

        // navigational

        public virtual ICollection<Wall> Walls { get; private set; }
    }
}