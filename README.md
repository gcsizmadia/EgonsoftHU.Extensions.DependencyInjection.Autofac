# Egonsoft.HU DependencyInjection Extensions for Autofac

[![GitHub](https://img.shields.io/github/license/gcsizmadia/EgonsoftHU.Extensions.DependencyInjection.Autofac?label=License)](https://opensource.org/licenses/MIT)
[![Nuget](https://img.shields.io/nuget/v/EgonsoftHU.Extensions.DependencyInjection.Autofac?label=NuGet)](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac)
[![Nuget](https://img.shields.io/nuget/dt/EgonsoftHU.Extensions.DependencyInjection.Autofac?label=Downloads)](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac)

A dependency module (derived from `Autofac.Module`) that discovers and registers all other dependency modules (derived from `Autofac.Module`).

The dependency modules can also have shared dependencies injected into them, e.g. `IConfiguration`, `IHostEnvironment` and even `IServiceCollection`.

## Table of Contents

* [Introduction](#introduction)
* [Releases](#releases)
* [Autofac version](#autofac-version)
* [Summary](#summary)
* [Assembly registry](#assembly-registry)
  + [Option #1 - Use the default assembly registry](#option-1---use-the-default-assembly-registry)
  + [Option #2 - Use the pre-initialized default assembly registry](#option-2---use-the-pre-initialized-default-assembly-registry)
  + [Option #3 - Use your custom assembly registry (interface + parameterless ctor)](#option-3---use-your-custom-assembly-registry-interface--parameterless-ctor)
  + [Option #4 - Use your custom assembly registry (interface only)](#option-4---use-your-custom-assembly-registry-interface-only)
  + [Option #5 - Use your custom assembly registry (it will be used through reflection)](#option-5---use-your-custom-assembly-registry-it-will-be-used-through-reflection)
* [`AutofacServiceProviderFactoryDecorator`](#autofacserviceproviderfactorydecorator)
* [`ContainerBuilder` extension methods](#containerbuilder-extension-methods)
  + [Configuring an assembly registry](#configuring-an-assembly-registry)
  + [DI for the dependency modules](#di-for-the-dependency-modules)
* [Examples](#examples)
  + [Example for MAUI](#example-for-maui)
  + [Example for .NET 6+](#example-for-net-6)
  + [Example for .NET Framework 4.7.2+](#example-for-net-framework-472)
  + [Example without `AutofacServiceProviderFactoryDecorator`](#example-without-autofacserviceproviderfactorydecorator)
  + [Additional examples](#additional-examples)

## Introduction

The motivations behind this project are:
- to automatically discover and register all dependency modules in projects that are referenced by the startup project
- to be able to access `IConfiguration`, `IHostEnvironment` instances from within the dependency modules
- to be able to use `IServiceCollection` extension methods in them.

## Releases

You can download the package from [nuget.org](https://www.nuget.org/).
- [EgonsoftHU.Extensions.DependencyInjection.Autofac](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac)

You can find the release notes [here](https://github.com/gcsizmadia/EgonsoftHU.Extensions.DependencyInjection.Autofac/releases).

## Autofac version

|EgonsoftHU.Extensions.DependencyInjection.Autofac|1.0.0 - 2.0.0|3.0.0|4.0.0|
|:-|:-:|:-:|:-:|
|Autofac.Extensions.DependencyInjection<br/>*dependency type:*|-|8.0.0<br/>*direct / top-level*|10.0.0<br/>*direct / top-level*|
|Autofac<br/>*dependency type:*|4.9.4<br/>*direct / top-level*|6.4.0<br/>*indirect / transitive*|8.1.0<br/>*indirect / transitive*|
|Target frameworks|`netstandard2.0`<br/>`netstandard2.1`<br/>`net461`<br/>`netcoreapp3.1`<br/>`net6.0`|`netstandard2.0`<br/>`netstandard2.1`<br/>`net472`<br/>`netcoreapp3.1`<br/>`net6.0`|`netstandard2.0`<br/>`netstandard2.1`<br/>`net472`<br/>`net6.0`<br/>`net8.0`|

**Note:** .NET Framework target version changed because `Autofac.WebApi2` that can be used with `Autofac` 6+ is targeted only to .NET Framework 4.7.2.

## Summary

This solution uses an assembly registry,  a decorator class for `AutofacServiceProviderFactory` and an `Autofac` module.

The assembly registry contains all the relevant assemblies that may have a dependency module.

The decorator class (`EgonsoftHU.Extensions.DependencyInjection.AutofacServiceProviderFactoryDecorator`)
- captures the existing `IServiceCollection` instance  
  when the `IServiceProviderFactory<ContainerBuilder>.CreateBuilder(IServiceCollection)` method is called and
- shares a copy of it to the `Autofac` module below.

The `Autofac` module (`EgonsoftHU.Extensions.DependencyInjection.DependencyModule`)
1. checks all the assemblies in the assembly registry for types derived from `Autofac.Module` type
1. into a temporary `ContainerBuilder` instance
   - registers them, hence you will be able to inject dependencies into your dependency modules, e.g. `IConfiguration`
   - registers other dependencies you specified, e.g. `IConfiguration`
   - registers a copy of the shared `IServiceCollection` instance  
     (except if you already specified one, but then you need to handle possible service registration overrides)
1. builds the temporary `IContainer` instance
1. resolves the dependency modules from it
1. registers them into the actual `ContainerBuilder` instance
1. finally, the service registrations that are newly added to the shared `IServiceCollection` instance
   are populated into the actual `ContainerBuilder` instance.

**Note:** Step 2 - 4 will happen only if you use `ContainerBuilder.TreatModulesAsServices()` extension method directly
or indirectly by using `AutofacServiceProviderFactoryDecorator.CreateDefault()` method overloads.

## Assembly registry

There is a default implementation you can use.

The default implementation:
- requires your assembly file name prefixes (can be more than one), e.g. `YourPrefix`, `YourOtherPrefix`
- will search for assembly files with the following pattern: `YourPrefix.*.dll`, `YourOtherPrefix.*.dll`

**Note:** `EgonsoftHU` prefix is always added, hence all referenced `EgonsoftHU.*.dll` files will also be loaded.

You can provide your own implementation. The required steps to implement:
- **Initialization (ensure all relevant assemblies are loaded into the `AppDomain`)**  
  *either* using the parameterless ctor  
  *or* any other way that happens before configuring your implementation as the assembly registry
- **Getting all assemblies**  
  *either* implement this interface: `EgonsoftHU.Extensions.DependencyInjection.IAssemblyRegistry`  
  *or* provide a public parameterless instance method that can be called using reflection.  
     e.g. `public IEnumerable<Assembly> GetAssemblies()`  
     **Note:**
     The method name must be `GetAssemblies`.
     The return type can be any type that can be assigned to a variable of the `IEnumerable<Assembly>` type.

To configure the assembly registry, use one of the `ContainerBuilder` extension methods below.

### Option #1 - Use the default assembly registry

```csharp
/*
 * Use the default assembly registry.
 * Provide your assembly file name prefixes.
 */

builder.UseDefaultAssemblyRegistry(nameof(YourPrefix));
```

### Option #2 - Use the pre-initialized default assembly registry

```csharp
/*
 * Use the default assembly registry.
 * Provide your assembly file name prefixes.
 */

DefaultAssemblyRegistry.Initialize(nameof(YourPrefix));

builder.UseAssemblyRegistry(DefaultAssemblyRegistry.Current);
```

### Option #3 - Use your custom assembly registry (interface + parameterless ctor)

```csharp
/*
 * The custom assembly registry must:
 * - implement this interface: EgonsoftHU.Extensions.DependencyInjection.IAssemblyRegistry
 * - have a parameterless ctor that initializes the instance
 */
builder.UseAssemblyRegistry<YourCustomAssemblyRegistry>();
```

### Option #4 - Use your custom assembly registry (interface only)

```csharp
/*
 * The custom assembly registry must:
 * - implement this interface: EgonsoftHU.Extensions.DependencyInjection.IAssemblyRegistry
 */
IAssemblyRegistry assemblyRegistry = /* get an initialized instance of YourCustomAssemblyRegistry */
builder.UseAssemblyRegistry(assemblyRegistry);
```

### Option #5 - Use your custom assembly registry (it will be used through reflection)

```csharp
/*
 * The custom assembly registry must provide a public parameterless instance method:
 * - Name: GetAssemblies
 * - Return type: assignable to IEnumerable<Assembly>
 */
object assemblyRegistry = /* get an initialized instance of YourCustomAssemblyRegistry */
builder.UseAssemblyRegistry(assemblyRegistry);
```

## `AutofacServiceProviderFactoryDecorator`

This decorator class has static factory methods (`.CreateDefault()` overloads) so that the default usage will be very simple.

You can specify one or more assembly file name prefixes and zero or more dependencies (up to 4) to inject into the dependency modules.

**Example configuration with 1 prefix and 2 dependencies:**

```csharp
using EgonsoftHU.Extensions.DependencyInjection;

// Call the IHostBuilder.UseServiceProviderFactory() method.

builder.Host.UseServiceProviderFactory(
    hostBuilderContext => AutofacServiceProviderFactoryDecorator.CreateDefault(
        nameof(YourPrefix),
        hostBuilderContext.Configuration,
        hostBuilderContext.HostingEnvironment
    )
);
```

The functionally equivalent version:

```csharp
using EgonsoftHU.Extensions.DependencyInjection;

// Call the IHostBuilder.UseServiceProviderFactory() method.

builder.Host.UseServiceProviderFactory(
    hostBuilderContext =>
        new AutofacServiceProviderFactoryDecorator(
            containerBuilder =>
                containerBuilder
                    .UseDefaultAssemblyRegistry(nameof(YourPrefix))
                    .TreatModulesAsServices()
                    .RegisterModuleDependencyInstance(hostBuilderContext.Configuration)
                    .RegisterModuleDependencyInstance(hostBuilderContext.HostingEnvironment)
                    .RegisterModule<DependencyModule>()
        )
);
```

## `ContainerBuilder` extension methods

The following extension methods are available.

### Configuring an assembly registry

- `UseDefaultAssemblyRegistry(params string[] assemblyFileNamePrefixes)`  
- `UseRegistryAssembly<TAssemblyRegistry>() where TAssemblyRegistry : IAssemblyRegistry, new()`
- `UseRegistryAssembly(IAssemblyRegistry assemblyRegistry)`
- `UseRegistryAssembly(object assemblyRegistry)`

### DI for the dependency modules

- `ConfigureModuleOptions(Action<ModuleOptions> setupAction)`  
  The following options can be configured:  
  - `TreatModulesAsServices` (i.e. the dependency modules have dependencies, e.g. `IConfiguration`)  
    -> `false` (default) / `true`
  - `DependencyInjectionOption` (how to inject dependencies into the dependency modules)  
    -> `NoInjection` (default), `PropertyInjection`, `ConstructorInjection`
  - `OnModulesRegistered` (a delegate to execute after all the dependency modules are registered)  
    -> `Action<ContainerBuilder>`

- `TreatModulesAsServices()`  
  Configures `ModuleOptions`.
  - `TreatModulesAsServices` will be set to `true`
  - `DependencyInjectionOption` will be set to `ModuleDependencyInjectionOption.PropertyInjection`

- `RegisterModuleDependencyInstance<T>(T instance)`  
  Registers a module dependency into a temporary `ContainerBuilder` from which it will be resolved for the dependency modules.

**Please note:**

- Module dependency instances are registered this way:  
  `ContainerBuilder.RegisterInstance().ExternallyOwned()`
- You might register more instances of type `T` in which case set the type of the property (or the constructor parameter) to `IEnumerable<T>`.

## Examples

### Example for MAUI

This is an example for a .NET 8 MAUI project.

**MauiProgram.cs**

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        builder.ConfigureContainer(
            AutofacServiceProviderFactoryDecorator.CreateDefault(
                nameof(YourPrefix),
                // Here a cast is needed since the type of the Configuration property is
                // Microsoft.Extensions.Configuration.ConfigurationManager
                (IConfiguration)builder.Configuration
            )
        );

        // Alternatively, you can specify the type parameter.
        builder.ConfigureContainer(
            AutofacServiceProviderFactoryDecorator.CreateDefault<IConfiguration>(
                nameof(YourPrefix),
                builder.Configuration
            )
        );

        // rest is omitted for clarity
    }
}
```

### Example for .NET 6+

This is an example for an ASP.NET Core Web API project.

**Program.cs**

```csharp
using EgonsoftHU.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(
    hostBuilderContext => AutofacServiceProviderFactoryDecorator.CreateDefault(
        nameof(YourPrefix),
        hostBuilderContext.Configuration,
        hostBuilderContext.HostingEnvironment
    )
);

// rest is omitted for clarity
```

**CustomService.cs** (in an assembly the file name of which matches the `YourPrefix.*.dll` pattern)

```csharp
public class CustomService : ICustomService
{
    public CustomService(IOptions<MyCustomOptions> options, string environmentName)
    {
    }
}
```

**DependencyModule.cs** (in the same assembly in which `CustomService` is defined)

```csharp
using Autofac;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public sealed class DependencyModule : Module
{
    public required IConfiguration Configuration { get; set; }
    public required IHostEnvironment HostEnvironment { get; set; }
    public required IServiceCollection Services { get; set; }

    protected override void Load(ContainerBuilder builder)
    {
        Services
            .AddOptions<MyCustomOptions>()
            .Bind(Configuration.GetSection("MyCustomOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder
            .RegisterType<CustomService>()
            .As<ICustomService>()
            .InstancePerLifetimeScope()
            .WithParameter(new NamedParameter("environmentName", HostEnvironment.EnvironmentName));
    }
}
```

### Example for .NET Framework 4.7.2+

This is an example for an ASP.NET Web API 2 project.

**WebApiConfig.cs**

```csharp
using System.Web.Http;

namespace YourCompany.YourProduct.WebApi
{
    public static partial class WebApiConfig
    {
        public static void Register(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.ConfigureAutofac();
            // rest is omitted for clarity
        }
    }
}
```

**WebApiConfig.Extensions.Autofac.cs**

```csharp
using System.Configuration;
using System.Reflection;
using System.Web.Http;

using Autofac;
using Autofac.Integration.WebApi;

using EgonsoftHU.Extensions.DependencyInjection;

namespace YourCompany.YourProduct.WebApi
{
    public static partial class WebApiConfig
    {
        public static void ConfigureAutofac(HttpConfiguration httpConfiguration)
        {
            var builder = new ContainerBuilder();

            builder
                .UseDefaultAssemblyRegistry(nameof(YourCompany))
                .TreatModulesAsServices()
                .RegisterModuleDependencyInstance(ConfigurationManager.AppSettings)
                .RegisterModule<DependencyModule>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(httpConfiguration);

            IContainer container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
```

**CustomService.cs** (in an assembly the file name of which matches the `YourPrefix.*.dll` pattern)

```csharp
namespace YourCompany.YourProduct.WebApi
{
    public class CustomService : ICustomService
    {
        public CustomService(string environmentName)
        {
        }
    }
}
```

**DependencyModule.cs** (in the same assembly in which `CustomService` is defined)

```csharp
using System.Collections.Specialized;

using Autofac;

namespace YourCompany.YourProduct.WebApi
{
    public class DependencyModule : Module
    {
        public NameValueCollection AppSettings { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<CustomService>()
                .As<ICustomService>()
                .InstancePerLifetimeScope()
                .WithParameter(new NamedParameter("environmentName", AppSettings?["EnvironmentName"] ?? "N/A"));
        }
    }
}
```

### Example without `AutofacServiceProviderFactoryDecorator`

The `AutofacServiceProviderFactoryDecorator` is introduced in the `4.0.0` version of this package.

The example below is for the earlier package versions.  
Without the decorator you need to handle service registration override issues.

**Program.cs**

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .Host
    // UseSerilog()
    // registers Serilog.Extensions.Logging.SerilogLoggerFactory
    // as Microsoft.Extensions.Logging.ILoggerFactory
    .UseSerilog()
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(
        (hostBuilderContext, containerBuilder) =>
        {
            // This instance will be injected into all the dependency modules.
            // 
            // Since it does not contain the registration of SerilogLoggerFactory,
            // if IServiceCollection.AddLogging() extension method is called directly or indirectly
            // in the dependency modules then it will
            // register Microsoft.Extensions.Logging.LoggerFactory
            // as Microsoft.Extensions.Logging.ILoggerFactory.
            IServiceCollection services = new ServiceCollection();

            containerBuilder
                .UseDefaultAssemblyRegistry(nameof(Company))
                .TreatModulesAsServices()
                // To avoid overriding ILoggerFactory registration made by UseSerilog()
                // we remove all ILoggerFactory service registration
                // from our manually created IServiceCollection instance
                // before it is populated into the ContainerBuilder.
                .ConfigureModuleOptions(
                    options =>
                        options.OnModulesRegistered = _ => services.RemoveAll<ILoggerFactory>()
                )
                .RegisterModuleDependencyInstance(services)
                .RegisterModuleDependencyInstance(hostBuilderContext.Configuration)
                .RegisterModuleDependencyInstance(hostBuilderContext.HostingEnvironment)
                .RegisterModule<DependencyModule>();
        }
    );
```

### Additional examples

You can find some example projects in the solutions below:

- [Company.Product.sln (.NET Framework 4.7.2)](examples/NetFramework/Company.Product.sln)
  - `ASP.NET Web API 2`
- [Company.Product.sln (.NET 6 / .NET 8)](examples/NetCore/Company.Product.sln)
  - `ASP.NET Core Web API` (.NET 6) with `Microsoft.Extensions.Logging`
  - `ASP.NET Core Web API` (.NET 8) with `Serilog`
  - `MAUI` (.NET 8)
