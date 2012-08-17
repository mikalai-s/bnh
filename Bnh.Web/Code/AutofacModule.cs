using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Autofac;
using Bnh.Entities;
using Newtonsoft.Json;

namespace Bnh.Web.Code
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register<Configuration>(c => GetConfiguration()).InstancePerLifetimeScope();
            builder.Register<BleEntities>(c => new BleEntities()).InstancePerLifetimeScope();
        }

        private Configuration GetConfiguration()
        {
            return JsonConvert.DeserializeObject<Configuration>(
                File.ReadAllText(HttpContext.Current.Server.MapPath("~/config.json")));
        }
    }
}