// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using EgonsoftHU.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

ConfigureDefaultAssemblyRegistry();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .Host
    .UseServiceProviderFactory(
        hostBuilderContext =>
            AutofacServiceProviderFactoryDecorator.CreateDefault(
                nameof(Company),
                hostBuilderContext.Configuration,
                hostBuilderContext.HostingEnvironment
            )
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
    DefaultAssemblyRegistry.ConfigureLogging(
        logEvent =>
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(
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

            ILogger logger = loggerFactory.CreateLogger<DefaultAssemblyRegistry>();

            using (logger.BeginScope(logEvent.Properties))
            {
                logger.LogDebug(logEvent.MessageTemplate.Structured, logEvent.Arguments);
            }
        },
        LoggingLibrary.MicrosoftExtensionsLogging
    );
}
