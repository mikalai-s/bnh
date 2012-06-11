using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ms.Cms.Models
{
    public static class SceneTemplating
    {/*
        public static void CloneScene(Guid from, Guid to, CmsEntities db)
        {
            foreach (var wall in db.Walls.Where(w => w.OwnerGuidId == from))
            {
                var newWall = wall.Clone();
                newWall.Bricks.Clear();
                newWall.OwnerId = to;

                foreach (var brick in wall.Bricks)
                {
                    var newBrick = brick.Clone();
                    newBrick.Wall = newWall;
                }

                db.Walls.Add(newWall);
            }
        }*/
    }
}
