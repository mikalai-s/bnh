using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Core;
using SpellChecker.Net.Search.Spell;

namespace Bnh.Web.Helpers
{
    /// <summary>
    /// A simple derivee just to resolve corrrect connection strings
    /// </summary>
    public class MongoErrorLog : Elmah.MongoErrorLog
    {
        public MongoErrorLog(System.Collections.IDictionary config)
            : base(config)
        {
        }

        public override string GetConnectionString(System.Collections.IDictionary configSettings)
        {
            var config = DependencyResolver.Current.GetService<Config>();
            if (config == null)
            {
                throw new Exception("Config is not registered!");
            }

            return config.ConnectionStrings[(string)configSettings["connectionStringName"]];
        }
        
    }
}