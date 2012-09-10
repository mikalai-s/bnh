using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Bnh.Infrastructure.Repositories;

namespace Bnh.Infrastructure
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<EntityRepositories>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<RatingCalculator>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
