using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ms.Cms.Entities
{
    public static class SceneTemplating
    {
        public static void CloneScene(Guid from, Guid to, CmEntities db)
        {
            foreach (var wall in db.Walls.Where(w => w.OwnerId == from))
            {
                var newWall = wall.Clone();
                newWall.Bricks = null;
                newWall.OwnerId = to;

                foreach (var brick in wall.Bricks)
                {
                    var newBrick = brick.Clone();
                    newBrick.Wall = newWall;
                }

                db.Walls.AddObject(newWall);
            }
        }
    }
}
