using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Collections.Specialized;
using MongoDB.Bson;
using Cms.Core;

namespace MongoDB.Web.Providers
{
    /// <summary>
    /// MongoDB connection helpers
    /// </summary>
    internal class ConnectionUtils
    {
        /// <summary>
        /// Returns MongoDb collection specified in config setting ("collections")
        /// or default one
        /// </summary>
        /// <param name="config"></param>
        /// <param name="defaultCollection"></param>
        /// <returns></returns>
        public static MongoCollection<BsonDocument> GetCollection(NameValueCollection settings, IConfig config, string defaultCollection)
        {
            return GetDatabase(settings, config).GetCollection(settings["collection"] ?? defaultCollection);
        }

        /// <summary>
        /// Returns MongoDatabase instance using config settings.
        /// If "database" setting is not specified then it's assumed that
        /// connection string contains database name
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static MongoDatabase GetDatabase(NameValueCollection settings, IConfig config)
        {
            string database = settings["database"];
            return string.IsNullOrEmpty(database) ?
                MongoDatabase.Create(GetConnectionString(settings, config)) :
                MongoServer.Create(GetConnectionString(settings, config)).GetDatabase(database);
        }

        /// <summary>
        /// Returns connection string to MongoDb by checking whether "connectionString" 
        /// contains connection string name or connection string itself
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string GetConnectionString(NameValueCollection settings, IConfig config)
        {
            string connectionString = null;
            var nameOrConnectionString = settings["connectionString"];
            if (!string.IsNullOrEmpty(nameOrConnectionString))
            {
                connectionString = nameOrConnectionString;
                if (config.ConnectionStrings.ContainsKey(nameOrConnectionString))
                {
                    connectionString = config.ConnectionStrings[nameOrConnectionString];
                }
            }
            return connectionString ?? "mongodb://localhost";

        }
    }
}
