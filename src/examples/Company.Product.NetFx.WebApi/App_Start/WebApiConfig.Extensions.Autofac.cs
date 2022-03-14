// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Reflection;
using System.Web.Http;

using Autofac;
using Autofac.Integration.WebApi;

using EgonsoftHU.Extensions.DependencyInjection.Autofac;

namespace Company.Product.NetFx.WebApi
{
    partial class WebApiConfig
    {
        /// <summary>
        /// Configures Autofac as the dependency resolver for the current ASP.NET Web API application.
        /// </summary>
        /// <param name="httpConfiguration"></param>
        public static void ConfigureAutofac(this HttpConfiguration httpConfiguration)
        {
            var builder = new ContainerBuilder();

            builder.UseDefaultAssemblyRegistry(nameof(Company));
            builder.RegisterModule<DependencyModule>();

            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterApiControllers(assembly);
            builder.RegisterWebApiFilterProvider(httpConfiguration);

            IContainer container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
