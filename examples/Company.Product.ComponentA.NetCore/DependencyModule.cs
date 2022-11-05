// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Company.Product.ComponentA.NetCore
{
    public class DependencyModule : Module
    {
        public IConfiguration? Configuration { get; set; }

        public IHostEnvironment? HostEnvironment { get; set; }

        /// <summary>
        /// Registers services of ComponentA.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();

            if (Configuration is not null)
            {
                services
                    .AddOptions<ServiceAOptions>()
                    .Bind(Configuration.GetSection(nameof(ServiceA)))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
            }

            builder.Populate(services);

            builder
                .RegisterType<ServiceA>()
                .AsSelf()
                .SingleInstance()
                .WithParameter(new NamedParameter("environmentName", HostEnvironment?.EnvironmentName ?? "N/A"));
        }
    }
}
