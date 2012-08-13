using System;
using System.Linq;

using System.Reflection;
using System.Collections.Generic;
using System.Configuration;

using MongoDB.Driver.Linq;
using MongoDB.Bson;
using Ms.Cms.Models;

namespace Bnh.Entities
{
    public partial class BleEntities : IDisposable
    {
        public MongoRepository<string, Community> Communities { get; private set; }

        public MongoRepository<string, City> Cities { get; private set; }

        public MongoRepository<string, Review> Reviews { get; private set; }


        public BleEntities()
            : this("Bnh.Entities")
        {
        }


        public BleEntities(string nameOrConnectionString) 
        {
            var connectionString = nameOrConnectionString;
            if (ConfigurationManager.ConnectionStrings[nameOrConnectionString] != null)
            {
                connectionString = ConfigurationManager.ConnectionStrings[nameOrConnectionString].ConnectionString;
            }

            this.Communities = new MongoRepository<string, Community>(connectionString);
            this.Cities = new MongoRepository<string, City>(connectionString);

            this.Reviews = new MongoRepository<string, Review>(connectionString);
        }


        public void Dispose()
        {
        }
    }
}
