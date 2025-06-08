using Autofac;
using Data;
using Data.Utils;
using Service;
using Service.Utils;

namespace WebAPIDiceShop.Utils
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterModule(new ServiceModule());

        }
    }
}
