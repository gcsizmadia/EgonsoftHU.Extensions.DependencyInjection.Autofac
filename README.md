# Egonsoft.HU DependencyInjection Extensions for Autofac

[![GitHub](https://img.shields.io/github/license/gcsizmadia/EgonsoftHU.Extensions.DependencyInjection.Autofac?label=License)](https://opensource.org/licenses/MIT)
[![Nuget](https://img.shields.io/nuget/v/EgonsoftHU.Extensions.DependencyInjection.Autofac?label=NuGet)](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac)
[![Nuget](https://img.shields.io/nuget/dt/EgonsoftHU.Extensions.DependencyInjection.Autofac?label=Downloads)](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac)

A dependency module (derived from Autofac.Module) that discovers and registers all other dependency modules (derived from Autofac.Module).

## Introduction

The motivation behind this project is to automatically discover and register all dependency modules in projects that are referenced by the startup project.

## Releases

You can download the package from [nuget.org](https://www.nuget.org/).
- [EgonsoftHU.Extensions.DependencyInjection.Autofac](https://www.nuget.org/packages/EgonsoftHU.Extensions.DependencyInjection.Autofac)

You can find the release notes [here](https://github.com/gcsizmadia/EgonsoftHU.Extensions.DependencyInjection.Autofac/releases).

## Autofac version

This package references Autofac **4.9.4** nuget package but in your project you can use a newer version of Autofac.

Tested with Autofac **6.3.0** nuget package.

## Usage

Suppose you have a Web API project that uses a couple of your own libraries.

### Example solution

![C# Class Library](images/light/CSClassLibrary.png#gh-light-mode-only "C# Class Library") 
![C# Class Library](images/dark/CSClassLibrary.png#gh-dark-mode-only "C# Class Library") 
YourCompany.YourProduct.ComponentA.csproj\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Dependencies](images/light/ReferenceGroup.png#gh-light-mode-only "Dependencies") 
![Dependencies](images/dark/ReferenceGroup.png#gh-dark-mode-only "Dependencies") 
Dependencies\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Packages](images/light/PackageReference.png#gh-light-mode-only "Packages") 
![Packages](images/dark/PackageReference.png#gh-dark-mode-only "Packages") 
Packages\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Package Reference](images/light/PackageReference.png#gh-light-mode-only "Package Reference") 
![Package Reference](images/dark/PackageReference.png#gh-dark-mode-only "Package Reference") 
Autofac (6.3.0)\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![C# file](images/light/CSFileNode.png#gh-light-mode-only "C# File") 
![C# file](images/dark/CSFileNode.png#gh-dark-mode-only "C# File") 
DependencyModule.cs `// registers ServiceA into Autofac.ContainerBuilder`\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![C# file](images/light/CSFileNode.png#gh-light-mode-only "C# File") 
![C# file](images/dark/CSFileNode.png#gh-dark-mode-only "C# File") 
ServiceA.cs

![C# project](images/light/CSClassLibrary.png#gh-light-mode-only "C# Class Library") 
![C# project](images/dark/CSClassLibrary.png#gh-dark-mode-only "C# Class Library") 
YourCompany.YourProduct.ComponentB.csproj\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Dependencies](images/light/ReferenceGroup.png#gh-light-mode-only "Dependencies") 
![Dependencies](images/dark/ReferenceGroup.png#gh-dark-mode-only "Dependencies") 
Dependencies\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Packages](images/light/PackageReference.png#gh-light-mode-only "Packages") 
![Packages](images/dark/PackageReference.png#gh-dark-mode-only "Packages") 
Packages\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Package Reference](images/light/PackageReference.png#gh-light-mode-only "Package Reference") 
![Package Reference](images/dark/PackageReference.png#gh-dark-mode-only "Package Reference") 
Autofac (6.3.0)\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![C# file](images/light/CSFileNode.png#gh-light-mode-only "C# File") 
![C# file](images/dark/CSFileNode.png#gh-dark-mode-only "C# File") 
DependencyModule.cs `// registers ServiceB into Autofac.ContainerBuilder`\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![C# file](images/light/CSFileNode.png#gh-light-mode-only "C# File") 
![C# file](images/dark/CSFileNode.png#gh-dark-mode-only "C# File") 
ServiceB.cs

![C# Web Application](images/light/CSWebApplication.png#gh-light-mode-only "C# Web Application") 
![C# Web Application](images/dark/CSWebApplication.png#gh-dark-mode-only "C# Web Application") 
YourCompany.YourProduct.WebApi.csproj\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Dependencies](images/light/ReferenceGroup.png#gh-light-mode-only "Dependencies") 
![Dependencies](images/dark/ReferenceGroup.png#gh-dark-mode-only "Dependencies") 
Dependencies\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Packages](images/light/PackageReference.png#gh-light-mode-only "Packages") 
![Packages](images/dark/PackageReference.png#gh-dark-mode-only "Packages") 
Packages\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Package Reference](images/light/PackageReference.png#gh-light-mode-only "Package Reference") 
![Package Reference](images/dark/PackageReference.png#gh-dark-mode-only "Package Reference") 
Autofac (6.3.0)\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Package Reference](images/light/PackageReference.png#gh-light-mode-only "Package Reference") 
![Package Reference](images/dark/PackageReference.png#gh-dark-mode-only "Package Reference") 
Autofac.Extensions.DependencyInjection (7.2.0)\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Package Reference](images/light/PackageReference.png#gh-light-mode-only "Package Reference") 
![Package Reference](images/dark/PackageReference.png#gh-dark-mode-only "Package Reference") 
EgonsoftHU.Extensions.DependencyInjection.Autofac (1.0.0)\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Projects](images/light/Application.png#gh-light-mode-only "Projects") 
![Projects](images/dark/Application.png#gh-dark-mode-only "Projects") 
Projects\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Project reference](images/light/Application.png#gh-light-mode-only "Project Reference") 
![Project reference](images/dark/Application.png#gh-dark-mode-only "Project Reference") 
YourCompany.YourProduct.ComponentA\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Project reference](images/light/Application.png#gh-light-mode-only "Project Reference") 
![Project reference](images/dark/Application.png#gh-dark-mode-only "Project Reference") 
YourCompany.YourProduct.ComponentB\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Folder](images/light/FolderOpened.png#gh-light-mode-only "Folder") 
![Folder](images/dark/FolderOpened.png#gh-dark-mode-only "Folder") 
Services\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![C# file](images/light/CSFileNode.png#gh-light-mode-only "C# File") 
![C# file](images/dark/CSFileNode.png#gh-dark-mode-only "C# File") 
DependencyModule.cs `// registers ServiceC into Autofac.ContainerBuilder`\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![C# file](images/light/CSFileNode.png#gh-light-mode-only "C# File") 
![C# file](images/dark/CSFileNode.png#gh-dark-mode-only "C# File") 
ServiceC.cs\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![C# file](images/light/CSFileNode.png#gh-light-mode-only "C# File") 
![C# file](images/dark/CSFileNode.png#gh-dark-mode-only "C# File") 
Program.cs\
![Placeholder](images/light/Placeholder.png#gh-light-mode-only) 
![Placeholder](images/dark/Placeholder.png#gh-dark-mode-only) 
![C# file](images/light/CSFileNode.png#gh-light-mode-only "C# File") 
![C# file](images/dark/CSFileNode.png#gh-dark-mode-only "C# File") 
Startup.cs

### Steps

You have to do the below steps either in your **Program.cs** file or in your **Startup.cs** file.

#### Step 1

Configure an assembly registry. There is a default implementation you can use.

The default implementation:
- requires your assembly file name prefixes
- will search for assembly files with the following pattern: `YourPrefix.*.dll`

You can provide your own implementation:
- implement this interface: `EgonsoftHU.Extensions.DependencyInjection.Autofac.IAssemblyRegistry`
- initialize the instance either in the parameterless ctor or in a custom method.

#### Step 2

Register the module that will discover and register all other modules.

### Example Program.cs

```C#
using Autofac;
using Autofac.Extensions.DependencyInjection;

using EgonsoftHU.Extensions.DependencyInjection.Autofac;

namespace YourCompany.YourProduct.WebApi
{
    public class Program
    {
        // rest omitted for clarity

        static IHost CreateHost(string[] args)
        {
            return
                Host.CreateDefaultBuilder(args)
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureContainer<ContainerBuilder>(
                        builder =>
                        {
                            // Step #1: Configure assembly registry - option #1
                            // Use the default assembly registry.
                            // Provide your assembly file name prefixes.
                            AssemblyRegistryConfiguration.UseDefaultAssemblyRegistry(nameof(YourCompany));

                            // Step #1: Configure assembly registry - option #2
                            // The custom assembly registry must:
                            // - implement this interface: EgonsoftHU.Extensions.DependencyInjection.Autofac.IAssemblyRegistry
                            // - have a parameterless ctor that initializes the instance
                            AssemblyRegistryConfiguration.UseAssemblyRegistry<YourCustomAssemblyRegistry>();

                            // Step #1: Configure assembly registry - option #3
                            // The custom assembly registry must:
                            // - implement this interface: EgonsoftHU.Extensions.DependencyInjection.Autofac.IAssemblyRegistry
                            IAssemblyRegistry assemblyRegistry = /* get an initialized instance of YourCustomAssemblyRegistry */
                            AssemblyRegistryConfiguration.UseAssemblyRegistry(assemblyRegistry);

                            // Step #2: Register the module that will discover and register all other modules.
                            builder.RegisterModule<EgonsoftHU.Extensions.DependencyInjection.Autofac.DependencyModule>();
                        }
                    )
                    .Build();
        }
    }
}
```

### Example Startup.cs

```C#
using Autofac;

using EgonsoftHU.Extensions.DependencyInjection.Autofac;

namespace YourCompany.YourProduct.WebApi
{
    public class Startup
    {
        // rest omitted for clarity.

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Step #1: Configure assembly registry - option #1
            // Use the default assembly registry.
            // Initialize your assembly file name prefixes.
            AssemblyRegistryConfiguration.UseDefaultAssemblyRegistry(nameof(YourCompany));

            // Step #1: Configure assembly registry - option #2
            // The custom assembly registry must:
            // - implement this interface: EgonsoftHU.Extensions.DependencyInjection.Autofac.IAssemblyRegistry
            // - have a parameterless ctor that initializes the instance
            AssemblyRegistryConfiguration.UseAssemblyRegistry<YourCustomAssemblyRegistry>();

            // Step #1: Configure assembly registry - option #3
            // The custom assembly registry must:
            // - implement this interface: EgonsoftHU.Extensions.DependencyInjection.Autofac.IAssemblyRegistry
            IAssemblyRegistry assemblyRegistry = /* get an initialized instance of YourCustomAssemblyRegistry */
            AssemblyRegistryConfiguration.UseAssemblyRegistry(assemblyRegistry);

            // Step #2: Register the module that will discover and register all other modules.
            builder.RegisterModule<EgonsoftHU.Extensions.DependencyInjection.Autofac.DependencyModule>();
        }
    }
}
```
