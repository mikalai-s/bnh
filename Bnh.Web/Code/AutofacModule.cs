using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Autofac;

using Newtonsoft.Json;

using Bnh.Core;
using Bnh.Infrastructure.Repositories;
using System.Web.Security;
using MongoDB.Web.Providers;

namespace Bnh.Web.Code
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register<Config>(c => GetConfiguration()).InstancePerLifetimeScope();
        }

        private Config GetConfiguration()
        {
            // config file
            var configFile = HttpContext.Current.Server.MapPath("~/config.json");
#if !DEBUG
            // release specific config file
            var privateConfigFile = HttpContext.Current.Server.MapPath("~/config.release.json");
#else
            // debug specific config file
            var privateConfigFile = HttpContext.Current.Server.MapPath("~/config.debug.json");
#endif
            if (!File.Exists(privateConfigFile))
                throw new Exception("Private config file doesn't exist!");

            // deserialize config file into object
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFile));

            // override its values from private config file
            JsonConvert.PopulateObject(File.ReadAllText(privateConfigFile), config);
            return config;
        }
    }
}