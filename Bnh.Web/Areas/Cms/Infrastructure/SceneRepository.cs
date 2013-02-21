using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cms.Models;
using Bnh.Core;
using Bnh.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Cms.Infrastructure
{
    public class SceneRepository : MongoRepository<Scene>
    {
        public SceneRepository(string connectionString)
            : base(connectionString)
        {
        }

        public Brick FindBrick(string brickId)
        {
            var scene = this.Collection
                .Find(Query.EQ("Walls.Bricks._id", BsonValue.Create(ObjectId.Parse(brickId))))
                .SetFields("Walls.Bricks")
                .Single();
            return scene.Walls.First().Bricks.First();
        }


        internal bool CanDeleteBrick(string linkedBrickId)
        {
            //var a = this.Collection.Find(
            //    Query.And(
            //        Query.EQ("SceneId", BsonValue.Create(ObjectId.Parse(Constants.LinkableBricksSceneId))),
            //        Query.EQ("Walls.$.Bricks.$."))
            // tODO: finish this
            return true;
        }

        internal void UpdateBrick(Brick brick)
        {
            var a = this.Collection.Update(
                Query.EQ("Walls.Bricks._id", BsonValue.Create(ObjectId.Parse(brick.BrickId))),
                Update.Set("Walls.$.Bricks.$", brick.ToBsonDocument()));
        }
    }
}
