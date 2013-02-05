using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Cms.Infrastructure;
using Autofac.Integration.Mvc;
using Cms.Core;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using Bnh.Core;
using Bnh.Infrastructure.Search;

namespace Bnh.Infrastructure
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<BnhRepositories>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<RatingCalculator>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<SearchProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ServerPathMapper>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.Register(c => GetConfiguration()).AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        private BnhConfig GetConfiguration()
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

            try
            {
                // deserialize config file into object
                var config = JsonConvert.DeserializeObject<BnhConfig>(File.ReadAllText(configFile));

                // override its values from private config file
                JsonConvert.PopulateObject(File.ReadAllText(privateConfigFile), config);

                return config;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                throw new ConfigurationErrorsException("Failed reading configuration file.", e);
            }
        }
    }
}
