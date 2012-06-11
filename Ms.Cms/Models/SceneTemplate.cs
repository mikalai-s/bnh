using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ms.Cms.Models
{
    [Table("SceneTemplates", Schema = "Cms")]
    public partial class SceneTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string IconUrl { get; set; }
    }
}
