using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Entities
{
    public static class BlEntitiesExtensions
    {
        public static IEnumerable<Wall> GetWallsFromEntityId(Guid id)
        {
            using (var db = new CmEntities())
            {
                // Convert to list to make sure context is open during request
                var walls = db.Walls.Where(w => w.OwnerId == id).OrderBy(w => w.Order).ToList();
                // ensure bricks while given db context is open
                walls.ForEach(w => w.Bricks.ToList());
                return walls;
            }
        }
    }
}