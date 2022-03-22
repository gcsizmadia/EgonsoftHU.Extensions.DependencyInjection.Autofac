// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;
using Autofac.Extensions.DependencyInjection;

using EgonsoftHU.Extensions.DependencyInjection.Autofac;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Company.Product.NetCore.WebApi
{
    /// <summary>
    /// Provides the application entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">Command-line arguments passed when the process started.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates a host builder.
        /// </summary>
        /// <param name="args">Command-line arguments passed when the process started.</param>
        /// <returns>the host builder instance.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(
                        webHostBuilder =>
                        {
                            webHostBuilder.UseStartup<Startup>();
                        }
                    )
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureContainer<ContainerBuilder>(
                        builder =>
                        {
                            builder.UseDefaultAssemblyRegistry(nameof(Company));
                            builder.RegisterModule<DependencyModule>();
                        }
                    );
        }
    }
}
