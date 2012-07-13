using System;
using System.Linq;

using System.Reflection;
using System.Collections.Generic;
using System.Configuration;

using MongoDB.Driver.Linq;
using MongoDB.Bson;

namespace Ms.Cms.Models
{
    public partial class CmsEntities : IDisposable
    {
        public CmsEntities(string nameOrConnectionString) 
        {
            var connectionString = nameOrConnectionString;
            if (ConfigurationManager.ConnectionStrings[nameOrConnectionString] != null)
            {
                connectionString = ConfigurationManager.ConnectionStrings[nameOrConnectionString].ConnectionString;
            }

            this.Scenes = new SceneRepository(connectionString);
            this.BrickContents = new MongoRepository<string, BrickContent>(connectionString);
        }

        public CmsEntities()
            : this("Ms.Cms")
        {
        }

        public SceneRepository Scenes { get; private set; }
        public MongoRepository<string, BrickContent> BrickContents { get; private set; }

        public void Dispose()
        {
        }
    }
}
