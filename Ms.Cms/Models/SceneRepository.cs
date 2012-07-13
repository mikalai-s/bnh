using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Ms.Cms.Models
{
    public class SceneRepository : MongoRepository<ObjectId, Scene>
    {
        public SceneRepository(string connectionString) : base(connectionString)
        {
        }
    }
}