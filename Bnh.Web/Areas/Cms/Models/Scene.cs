using System;
using System.Collections.Generic;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cms.Models
{
    public class Scene
    {
        public ICollection<Wall> Walls
        {
            get { return this._walls ?? (this._walls = new List<Wall>()); }
            set { this._walls = value; }
        } ICollection<Wall> _walls = null;
    }
}