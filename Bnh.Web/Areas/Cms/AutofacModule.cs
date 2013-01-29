using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Bnh.Cms.Models;
using Bnh.Cms.Repositories;
using Bnh.Infrastructure.Repositories;

namespace Bnh.Cms
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<CmsRepos>().InstancePerLifetimeScope();
        }
    }
}
