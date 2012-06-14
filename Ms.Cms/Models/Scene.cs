using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Ms.Cms.Models.Attributes;

namespace Ms.Cms.Models
{
    [Table("Scene", Schema = "Cms")]
    public partial class Scene
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

        [NonJsExposable]
        public virtual ICollection<Wall> Walls { get; private set; }
    }
}