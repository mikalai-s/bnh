using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ms.Cms.Entities
{
    public partial class SharedBrick
    {
        // TODO: maybe update cm model to load the Share in context of loading entire entity
        public Brick Share
        {
            get
            {
                using (var cm = new CmEntities())
                {
                    return cm.Bricks.First(b => b.Id == this.SharedBrickId);
                }
            }
        }
    }
}
