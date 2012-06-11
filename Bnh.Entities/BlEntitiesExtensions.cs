using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ms.Cms.Models;

namespace Bnh.Entities
{
    public static class BlEntitiesExtensions
    {
        public static IEnumerable<Wall> GetWallsFromEntityId(Guid id)
        {
            var db = new CmsEntities();

            // Convert to list to make sure context is open during request
            var walls = db.Scenes
                .Where(s => s.OwnerGuidId == id)
                .SelectMany(s => s.Walls)
                .OrderBy(w => w.Order).ToList();
            // ensure bricks while given db context is open
            walls.ForEach(w => w.Bricks.ToList());
            return walls;
        }
    }
}