// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;
using Autofac.Extensions.DependencyInjection;

using EgonsoftHU.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            ConfigureDefaultAssemblyRegistry();
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
                        (hostBuilderContext, builder) =>
                        {
                            IServiceCollection services = new ServiceCollection();

                            builder
                                .UseDefaultAssemblyRegistry(nameof(Company))
                                .TreatModulesAsServices()
                                .RegisterModuleDependencyInstance(services)
                                .RegisterModuleDependencyInstance(hostBuilderContext.Configuration)
                                .RegisterModuleDependencyInstance(hostBuilderContext.HostingEnvironment)
                                .RegisterModule<DependencyModule>();
                        }
                    );
        }

        private static void ConfigureDefaultAssemblyRegistry()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(
                loggingBuilder =>
                loggingBuilder
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddJsonConsole(
                        options =>
                        {
                            options.IncludeScopes = true;
                            options.JsonWriterOptions = new() { Indented = true };
                        }
                    )
                    .AddDebug()
            );

            DefaultAssemblyRegistry.ConfigureLogging(
                logEvent =>
                {
                    ILogger logger = loggerFactory.CreateLogger<DefaultAssemblyRegistry>();

                    using (logger.BeginScope(logEvent.Properties))
                    {
                        logger.LogDebug(logEvent.MessageTemplate.Structured, logEvent.Arguments);
                    }
                },
                LoggingLibrary.MicrosoftExtensionsLogging
            );
        }
    }
}
