using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bnh.Entities
{
    public static class BlEntitiesExtensions
    {
        public static Wall GetWall(this Community entity)
        {
            return GetEntityWall(entity.Id);
        }

        public static Wall GetWall(this Builder entity)
        {
            return GetEntityWall(entity.Id);
        }

        public static IEnumerable<Brick> GetWallBricks(this Community entity)
        {
            return GetBricksFromEntityWall(entity.Id);
        }

        public static IEnumerable<Brick> GetWallBricks(this Builder entity)
        {
            return GetBricksFromEntityWall(entity.Id);
        }

        private static Wall GetEntityWall(Guid entityId)
        {
            using (var db = new CmEntities())
            {
                var wall = db.Walls.FirstOrDefault(w => w.OwnerId == entityId);
                if(wall == null)
                {
                    wall = db.Walls.CreateObject();
                    wall.OwnerId = entityId;
                    wall.Title = string.Empty;
                    db.Walls.AddObject(wall);
                    db.SaveChanges();
                }
                return wall;
            }
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