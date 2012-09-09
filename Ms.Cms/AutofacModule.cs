using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Bnh.Infrastructure.Repositories;
using Ms.Cms.Models;

namespace Ms.Cms
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<CmsEntities>().InstancePerLifetimeScope();
        }
    }
}
