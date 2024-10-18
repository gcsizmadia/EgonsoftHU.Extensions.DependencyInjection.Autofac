// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;
using Autofac.Extensions.DependencyInjection;

using EgonsoftHU.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

ConfigureDefaultAssemblyRegistry();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(
        (hostBuilderContext, containerBuilder) =>
        {
            IServiceCollection services = new ServiceCollection();

            containerBuilder
                .UseDefaultAssemblyRegistry(nameof(Company))
                .TreatModulesAsServices()
                .RegisterModuleDependencyInstance(services)
                .RegisterModuleDependencyInstance(hostBuilderContext.Configuration)
                .RegisterModuleDependencyInstance(hostBuilderContext.HostingEnvironment)
                .RegisterModule<DependencyModule>();
        }
    );

builder.Services.AddControllers().AddControllersAsServices();

WebApplication app = builder.Build();

app
    .UseRouting()
    .UseAuthorization()
    .UseEndpoints(
        endpoints =>
        {
            endpoints.MapGet(
                "/",
                async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                }
            );

            endpoints.MapControllers();
        }
    );

app.Run();

static void ConfigureDefaultAssemblyRegistry()
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
#pragma warning disable CA2254 // Template should be a static expression
                logger.LogDebug(logEvent.MessageTemplate.Structured, logEvent.Arguments);
#pragma warning restore CA2254 // Template should be a static expression
            }
        },
        LoggingLibrary.MicrosoftExtensionsLogging
    );
}
