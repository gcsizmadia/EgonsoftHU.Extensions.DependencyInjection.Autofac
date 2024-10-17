// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Configuration;
using System.Reflection;
using System.Web.Http;

using Autofac;
using Autofac.Integration.WebApi;

using EgonsoftHU.Extensions.DependencyInjection;
using EgonsoftHU.Extensions.Logging;

using Serilog;

namespace Company.Product.NetFx.WebApi
{
    public partial class WebApiConfig
    {
        /// <summary>
        /// Configures Autofac as the dependency resolver for the current ASP.NET Web API application.
        /// </summary>
        /// <param name="httpConfiguration"></param>
        public static void ConfigureAutofac(this HttpConfiguration httpConfiguration)
        {
            ILogger logger = Log.Logger.ForContext<DefaultAssemblyRegistry>();

            DefaultAssemblyRegistry.ConfigureLogging(
                logEvent =>
                logger
                    .ForContext(PropertyBagEnricher.Create().AddRange(logEvent.Properties))
                    .Verbose(logEvent.MessageTemplate.Structured, logEvent.Arguments)
            );

            var builder = new ContainerBuilder();

            builder
                .UseDefaultAssemblyRegistry(nameof(Company))
                .TreatModulesAsServices()
                .RegisterModuleDependencyInstance(ConfigurationManager.AppSettings)
                .RegisterModule<DependencyModule>();

            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterApiControllers(assembly);
            builder.RegisterWebApiFilterProvider(httpConfiguration);

            IContainer container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
