# Egonsoft.HU DependencyInjection Extensions for Autofac

[![GitHub](https://img.shields.io/github/license/gcsizmadia/EgonsoftHU.Extensions.DependencyInjection.Autofac?label=License)](https://opensource.org/licenses/MIT)
[![Nuget](https://img.shields.io/nuget/v/EgonsoftHU.Extensions.DependencyInjection.Autofac?label=NuGet)](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac)
[![Nuget](https://img.shields.io/nuget/dt/EgonsoftHU.Extensions.DependencyInjection.Autofac?label=Downloads)](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac)

A dependency module (derived from Autofac.Module) that discovers and registers all other dependency modules (derived from Autofac.Module).

Please note: `EgonsoftHU.Extensions.DependencyInjection.Abstractions` project moved to [its own repository](https://github.com/gcsizmadia/EgonsoftHU.Extensions.DependencyInjection.Abstractions).

## Table of Contents

- [Introduction](#introduction)
- [Releases](#releases)
- [Autofac version](#autofac-version)
- [Summary](#summary)
- [Instructions](#instructions)
  - [Instructions for .NET Core 3.1 / .NET 5 / .NET 6](#instructions-for-net-core-31---net-5---net-6)
  - [Instructions for .NET Framework 4.6.1+](#instructions-for-net-framework-461-)
  - [Usage option #1 - Use the default assembly registry](#usage-option--1---use-the-default-assembly-registry)
  - [Usage option #2 - Use your custom assembly registry (interface + parameterless ctor)](#usage-option--2---use-your-custom-assembly-registry--interface---parameterless-ctor-)
  - [Usage option #3 - Use your custom assembly registry (interface only)](#usage-option--3---use-your-custom-assembly-registry--interface-only-)
  - [Usage option #4 - Use your custom assembly registry (it will be used through reflection)](#usage-option--4---use-your-custom-assembly-registry--it-will-be-used-through-reflection-)
- [Examples](#examples)
  - [Example output - .NET Framework 4.8](#example-output---net-framework-48)
  - [Example output - .NET 6](#example-output---net-6)

## Introduction

The motivation behind this project is to automatically discover and register all dependency modules in projects that are referenced by the startup project.

## Releases

You can download the package from [nuget.org](https://www.nuget.org/).
- [EgonsoftHU.Extensions.DependencyInjection.Autofac](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac)

You can find the release notes [here](https://github.com/gcsizmadia/EgonsoftHU.Extensions.DependencyInjection.Autofac/releases).

## Autofac version

This package references Autofac **4.9.4** nuget package but in your project you can use a newer version of Autofac.

Tested with Autofac **6.3.0** nuget package.

## Summary

To use this solution you need to do 2 things.

***First***, configure an assembly registry. There is a default implementation you can use.

The default implementation:
- requires your assembly file name prefixes, e.g. `YourPrefix`
- will search for assembly files with the following pattern: `YourPrefix.*.dll`

You can provide your own implementation. The required steps to implement:
- **Initialization (ensure all relevant assemblies are loaded into the `AppDomain`)**  
  *either* using the parameterless ctor  
  *or* any other way that happens before configuring your implementation as the assembly registry
- **Getting all assemblies**  
  *either* implement this interface: `EgonsoftHU.Extensions.DependencyInjection.Autofac.IAssemblyRegistry`  
  *or* provide a public parameterless instance method that can be called using reflection.  
     e.g. `public IEnumerable<Assembly> GetAssemblies()`  
     **Note:**
     The method name must be `GetAssemblies`.
     The return type can be any type that can be assigned to a variable of the `IEnumerable<Assembly>` type.

***Finally***, register the module that will discover and register all other modules.

## Instructions

The usage options are the same for both .NET Framework and .NET Core 3.1 / .NET 5 / .NET 6.
The difference is where the magic happens.

### Instructions for .NET Core 3.1 / .NET 5 / .NET 6

***First***, install the *EgonsoftHU.Extensions.DependencyInjection.Autofac* [NuGet package](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac).
```
dotnet add package EgonsoftHU.Extensions.DependencyInjection.Autofac
```

***Next***, add `ConfigureContainer<ContainerBuilder>()` to the Generic Host in `CreateHostBuilder()`.
```C#
using Autofac;
using Autofac.Extensions.DependencyInjection;

using EgonsoftHU.Extensions.DependencyInjection.Autofac;

namespace YourCompany.YourProduct.WebApi
{
    public class Program
    {
        // rest omitted for clarity

        static IHostBuilder CreateHostBuilder(string[] args)
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
                    .ConfigureContainer<ContainerBuilder>( // <-- Add this method call
                        builder =>
                        {
                            // here comes the magic
                        }
                    );
        }
    }
}
```

***Alternatively***, you can add `ConfigureContainer(ContainerBuilder builder)` to your `Startup.cs` file.

```C#
using Autofac;

using EgonsoftHU.Extensions.DependencyInjection.Autofac;

namespace YourCompany.YourProduct.WebApi
{
    public class Startup
    {
        // rest omitted for clarity.

        public void ConfigureContainer(ContainerBuilder builder) // <-- Add this method
        {
            // here comes the magic
        }
    }
}
```

***Finally***, replace the `// here comes the magic` comment with one of the usage options.

### Instructions for .NET Framework 4.6.1+

***First***, in the Package Manager Console install the *EgonsoftHU.Extensions.DependencyInjection.Autofac* [NuGet package](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac).
```pwsh
Install-Package EgonsoftHU.Extensions.DependencyInjection.Autofac
```

***Then***, locate where you create Autofac's `ContainerBuilder`.

***Then***, choose one of the usage options below.

***Finally***, add the required code right after the creation of `ContainerBuilder`.

**Note:** You can find an example in the `Company.Product.NetFx.WebApi` project.
Check out the [`WebApiConfig.Extensions.Autofac.cs`](examples/Company.Product.NetFx.WebApi/App_Start/WebApiConfig.Extensions.Autofac.cs) file.
It contains configuring Autofac as the ASP.NET dependency resolver and also the [usage option #1](#usage-option--1---use-the-default-assembly-registry).

### Usage option #1 - Use the default assembly registry

```C#
/*
 * Step #1: Configure assembly registry
 *
 * Use the default assembly registry.
 * Provide your assembly file name prefixes.
 */

// if the DefaultAssemblyRegistry is not initialized yet
builder.UseDefaultAssemblyRegistry(nameof(YourCompany));

// if you already initialized the DefaultAssemblyRegistry by calling its Initialize() method
builder.UseAssemblyRegistry(DefaultAssemblyRegistry.Current);

// Step #2: Register the module that will discover and register all other modules.
builder.RegisterModule<EgonsoftHU.Extensions.DependencyInjection.Autofac.DependencyModule>();
```

### Usage option #2 - Use your custom assembly registry (interface + parameterless ctor)

```C#
/*
 * Step #1: Configure assembly registry
 *
 * The custom assembly registry must:
 * - implement this interface: EgonsoftHU.Extensions.DependencyInjection.Autofac.IAssemblyRegistry
 * - have a parameterless ctor that initializes the instance
 */
builder.UseAssemblyRegistry<YourCustomAssemblyRegistry>();

// Step #2: Register the module that will discover and register all other modules.
builder.RegisterModule<EgonsoftHU.Extensions.DependencyInjection.Autofac.DependencyModule>();
```

### Usage option #3 - Use your custom assembly registry (interface only)

```C#
/*
 * Step #1: Configure assembly registry
 *
 * The custom assembly registry must:
 * - implement this interface: EgonsoftHU.Extensions.DependencyInjection.Autofac.IAssemblyRegistry
 */
IAssemblyRegistry assemblyRegistry = /* get an initialized instance of YourCustomAssemblyRegistry */
builder.UseAssemblyRegistry(assemblyRegistry);

// Step #2: Register the module that will discover and register all other modules.
builder.RegisterModule<EgonsoftHU.Extensions.DependencyInjection.Autofac.DependencyModule>();
```

### Usage option #4 - Use your custom assembly registry (it will be used through reflection)

```C#
/*
 * Step #1: Configure assembly registry
 *
 * The custom assembly registry must provide a public parameterless instance method:
 * - Name: GetAssemblies
 * - Return type: assignable to IEnumerable<Assembly>
 */
object assemblyRegistry = /* get an initialized instance of YourCustomAssemblyRegistry */
builder.UseAssemblyRegistry(assemblyRegistry);

// Step #2: Register the module that will discover and register all other modules.
builder.RegisterModule<EgonsoftHU.Extensions.DependencyInjection.Autofac.DependencyModule>();
```

## Examples

Check out the `examples` folder that contains an `Examples.sln` solution file with the following projects:

|Type|Project&nbsp;Name&nbsp;/&nbsp;Target&nbsp;Framework|Description|
|:-:|-|-|
|![C# Class Library](images/light/CSClassLibrary.png#gh-light-mode-only "C# Class Library")![C# Class Library](images/dark/CSClassLibrary.png#gh-dark-mode-only "C# Class Library")|`Company.Product.ComponentA`<br/>.NET&nbsp;Standard&nbsp;2.0|Contains `ServiceA` and a `DependencyModule` that registers it.|
|![C# Class Library](images/light/CSClassLibrary.png#gh-light-mode-only "C# Class Library")![C# Class Library](images/dark/CSClassLibrary.png#gh-dark-mode-only "C# Class Library")|`Company.Product.ComponentB`<br/>.NET&nbsp;Standard&nbsp;2.0|Contains `ServiceB` and a `DependencyModule` that registers it.|
|![C# Web Application](images/light/CSWebApplication.png#gh-light-mode-only "C# Web Application")![C# Web Application](images/dark/CSWebApplication.png#gh-dark-mode-only "C# Web Application")|`Company.Product.NetFx.WebApi`<br/>.NET&nbsp;Framework&nbsp;4.8|Uses `ServiceA` and `ServiceB` and also contains and uses `ServiceC` and a `DependencyModule` that registers it.|
|![C# Web Application](images/light/CSWebApplication.png#gh-light-mode-only "C# Web Application")![C# Web Application](images/dark/CSWebApplication.png#gh-dark-mode-only "C# Web Application")|`Company.Product.NetCore.WebApi`<br/>.NET&nbsp;6.0|Uses `ServiceA` and `ServiceB` and also contains and uses `ServiceC` and a `DependencyModule` that registers it.|

### Example output - .NET Framework 4.8

Navigating to the `~/api/tests` API endpoint should display this result:
```json
{
  "ServiceA": "Hello from Company.Product.ComponentA.ServiceA",
  "ServiceB": "Hello from Company.Product.ComponentB.ServiceB",
  "ServiceC": "Hello from Company.Product.NetFx.WebApi.Services.ServiceC",
  "Assemblies": [
    "Company.Product.ComponentA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
    "Company.Product.ComponentB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
    "Company.Product.NetFx.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
  ]
}
```

### Example output - .NET 6

Navigating to the `~/api/tests` API endpoint should display this result:
```json
{
  "serviceA": "Hello from Company.Product.ComponentA.ServiceA",
  "serviceB": "Hello from Company.Product.ComponentB.ServiceB",
  "serviceC": "Hello from Company.Product.NetCore.WebApi.Services.ServiceC",
  "assemblies": [
    "Company.Product.ComponentA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
    "Company.Product.ComponentB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
    "Company.Product.NetCore.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
  ]
}
```
