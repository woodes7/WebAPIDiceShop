using Autofac;
using Data;
using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Utils
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(GetType().Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterModule(new DataModule());

        }
    }
}
