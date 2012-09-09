using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Infrastructure.Repositories;
using MongoDB.Bson;

namespace Ms.Cms.Models
{
    public class SceneRepository : MongoRepository<Scene>
    {
        public SceneRepository(string connectionString) : base(connectionString)
        {
        }
    }
}