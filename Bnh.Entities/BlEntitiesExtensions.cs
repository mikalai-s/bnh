using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Entities
{
    public static class BlEntitiesExtensions
    {
        public static IEnumerable<Brick> GetWallBricks(this Community entity)
        {
            return GetBricksFromEntityWall(entity.Id);
        }

        public static IEnumerable<Brick> GetWallBricks(this Builder entity)
        {
            return GetBricksFromEntityWall(entity.Id);
        }

        private static IEnumerable<Brick> GetBricksFromEntityWall(Guid entityId)
        {
            using (var db = new CmEntities())
            {
                return (from wall in db.Walls
                        from brick in wall.Bricks
                        where wall.OwnerId == entityId
                        select brick).OrderBy(b => b.Order).ToList();
            }
        }
    }
}