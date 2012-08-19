using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Autofac;

using Newtonsoft.Json;

using Bnh.Core;
using Bnh.Infrastructure.Repositories;

namespace Bnh.Web.Code
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register<Configuration>(c => GetConfiguration()).InstancePerLifetimeScope();
            builder.RegisterType<EntityRepositories>().As<IEntityRepositories>().InstancePerLifetimeScope();
        }

        private Configuration GetConfiguration()
        {
            // config file
            var configFile = HttpContext.Current.Server.MapPath("~/config.json");
#if RELEASE
            // release specific config file
            var privateConfigFile = HttpContext.Current.Server.MapPath("~/config.release.json");
#else
            // debug specific config file
            var privateConfigFile = HttpContext.Current.Server.MapPath("~/config.debug.json");
#endif
            if (!File.Exists(privateConfigFile))
                throw new Exception("Private config file doesn't exist!");

            // deserialize config file into object
            var config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configFile));

            // override its values from private config file
            JsonConvert.PopulateObject(File.ReadAllText(privateConfigFile), config);
            return config;
        }
    }
}