using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ms.Cms.Entities;

namespace Bnh.Entities
{
    internal interface IWallOwners
    {
        IEnumerable<Wall> Walls { get; }
    }

    public partial class Community : IWallOwners
    {
        public IEnumerable<Wall> Walls
        {
            get { return BlEntitiesExtensions.GetWallsFromEntityId(Id); }
        }
    }

    public partial class Builder : IWallOwners
    {
        public IEnumerable<Wall> Walls
        {
            get { return BlEntitiesExtensions.GetWallsFromEntityId(Id); }
        }
    }
}
