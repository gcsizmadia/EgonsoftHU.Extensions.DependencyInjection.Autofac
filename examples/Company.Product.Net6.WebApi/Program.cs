// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;
using Autofac.Extensions.DependencyInjection;

using EgonsoftHU.Extensions.DependencyInjection;
using EgonsoftHU.Extensions.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

const string OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fffffff zzz} [{Level:u3}] [{SourceContext}]::[{SourceMember}] {Message:lj}{NewLine}{Properties}{NewLine}{Exception}";

Log.Logger =
    new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: OutputTemplate)
        .WriteTo.Debug(outputTemplate: OutputTemplate)
        .CreateBootstrapLogger();

ILogger logger = Log.Logger.ForContext<DefaultAssemblyRegistry>();

DefaultAssemblyRegistry.ConfigureLogging(
    logEvent =>
    logger
        .ForContext(PropertyBagEnricher.Create().AddRange(logEvent.Properties))
        .Verbose(logEvent.MessageTemplate.Structured, logEvent.Arguments)
);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(
        (hostBuilderContext, containerBuilder) =>
        {
            containerBuilder
                .UseDefaultAssemblyRegistry(nameof(Company))
                .TreatModulesAsServices()
                .RegisterModuleDependencyInstance(hostBuilderContext.Configuration)
                .RegisterModuleDependencyInstance(hostBuilderContext.HostingEnvironment)
                .RegisterModule<DependencyModule>();
        }
    )
    .UseSerilog(
        (hostBuilderContext, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(hostBuilderContext.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: OutputTemplate)
                .WriteTo.Debug(outputTemplate: OutputTemplate);
        }
    );

// Add services to the container.

builder.Services.AddControllers();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
