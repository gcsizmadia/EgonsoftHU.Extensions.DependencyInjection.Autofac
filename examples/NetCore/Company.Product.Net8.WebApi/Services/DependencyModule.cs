// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;

using EgonsoftHU.Extensions.Bcl;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Company.Product.Net8.WebApi.Services
{
    /// <summary>
    /// An Autofac dependency module that registers services of WebApi.
    /// </summary>
    public class DependencyModule : Module
    {
        public required IServiceCollection Services { get; set; }

        public required IConfiguration Configuration { get; set; }

        public required IHostEnvironment HostEnvironment { get; set; }

        /// <summary>
        /// Registers services of WebApi.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            if (HostEnvironment.IsDevelopment() || Configuration.GetValue("UseIServiceCollection", false))
            {
                Services.AddSingleton<ServiceD>();
            }
            else
            {
                builder
                    .RegisterType<ServiceD>()
                    .AsSelf()
                    .SingleInstance();
            }
        }
    }
}
