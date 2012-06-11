using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Ms.Cms.Models
{
    public partial class Wall : ICloneable
    {
        public virtual Wall Clone()
        {
            return new Wall
                {
                    Id = 0,
                    SceneId = this.SceneId,
                    Title = this.Title,
                    Width = this.Width,
                    Order = this.Order
                };
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
                    var p = property.GetCustomAttributes(typeof(DataMemberAttribute), false);
                    if (p.Length == 0)
                        continue;

                    p = property.GetCustomAttributes(typeof(XmlIgnoreAttribute), false);
                    if (p.Length > 0)
                        continue;

                    p = property.GetCustomAttributes(typeof(SoapIgnoreAttribute), false);
                    if (p.Length > 0)
                        continue;

                    p = property.GetCustomAttributes(typeof(BrowsableAttribute), false);
                    if (p.Length > 0 && !(p[0] as BrowsableAttribute).Browsable)
                        continue;

                    var value = property.GetValue(this, null);
                    if(value != null)
                        property.SetValue(clone, value, null);
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
