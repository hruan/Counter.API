using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;
using Autofac.Integration.WebApi;
using Counter.Api.AzureTableStorage;
using Counter.Api.Configurations;
using Owin;

namespace Counter.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Configuration(app, null);
        }

        public void Configuration(IAppBuilder app, Func<IContainer> containerBuilder)
        {
#if DEBUG
            LocalConfiguration.SetEnvironmentVariables();
#endif

            var container = (containerBuilder ?? DefaultContainerBuilder)();
            var config = new HttpConfiguration();
            EnableCors(config);
            config.MapHttpAttributeRoutes();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseWebApi(config);
        }

        private static IContainer DefaultContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<AzureTableStorageCounter>().AsImplementedInterfaces();
            builder.RegisterType<StorageConfigurationFromEnvironment>().AsImplementedInterfaces();
            builder.RegisterType<ApplicationConfigurationFromEnvironment>().AsImplementedInterfaces();

            return builder.Build();
        }

        private static void EnableCors(HttpConfiguration config)
        {
            var appConfig = new ApplicationConfigurationFromEnvironment();
            var cors = new EnableCorsAttribute(appConfig.GetAllowedOrigins(), "*", "*");
            config.EnableCors(cors);
        }
    }
}