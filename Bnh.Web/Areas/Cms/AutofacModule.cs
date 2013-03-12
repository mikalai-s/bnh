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

namespace Cms
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Repositories>().InstancePerLifetimeScope();
            builder.RegisterFilterProvider();           
        }
    }
}
