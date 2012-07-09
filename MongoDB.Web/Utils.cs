using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MongoDB.Web
{
    internal class Utils
    {
        internal static string GetConnectionString(NameValueCollection config)
        {
            string connectionString = null;
            var nameOrConnectionString = config["connectionString"];
            if (!string.IsNullOrEmpty(nameOrConnectionString))
            {
                connectionString = nameOrConnectionString;
                if (ConfigurationManager.ConnectionStrings[nameOrConnectionString] != null)
                {
                    connectionString = ConfigurationManager.ConnectionStrings[nameOrConnectionString].ConnectionString;
                }
            }
            return connectionString ?? "mongodb://localhost";
        }
    }
}
