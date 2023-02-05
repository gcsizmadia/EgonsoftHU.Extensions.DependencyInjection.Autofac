// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Company.Product.ComponentA.NetCore
{
    public class DependencyModule : Module
    {
        public IConfiguration? Configuration { get; set; }

        public IHostEnvironment? HostEnvironment { get; set; }

        public IServiceCollection? Services { get; set; }

        /// <summary>
        /// Registers services of ComponentA.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            if (Configuration is not null && Services is not null)
            {
                Services
                    .AddOptions<ServiceAOptions>()
                    .Bind(Configuration.GetSection(nameof(ServiceA)))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
            }

            builder
                .RegisterType<ServiceA>()
                .AsSelf()
                .SingleInstance()
                .WithParameter(
                    new NamedParameter(
                        "environmentName",
                        HostEnvironment?.EnvironmentName
                        ??
                        Configuration?.GetValue<string?>(nameof(IHostEnvironment.EnvironmentName))
                        ??
                        "N/A"
                    )
                );
        }
    }
}
