using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Bnh.Entities
{
    public partial class Wall : ICloneable
    {
        public virtual Wall Clone()
        {
            return Wall.CreateWall(0, this.OwnerId, this.Title, this.Width, this.Order);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }

    public partial class Brick : ICloneable
    {
        public virtual Brick Clone()
        {
            var type = this.GetType();
            var creator = type.GetMethod("Create" + type.Name);
            var clone = creator.Invoke(null, new object[] { 0, 0, this.Title, this.Width, this.Order }) as Brick;

            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.CanRead && property.CanWrite)
                {
                    property.SetValue(clone, property.GetValue(this, null), null);
                }
            }

            return clone;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
