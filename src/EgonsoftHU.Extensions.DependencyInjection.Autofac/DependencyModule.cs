// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using Module = Autofac.Module;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    /// <summary>
    /// Registers all other dependency modules that are derived from <see cref="Module"/> type.
    /// </summary>
    public class DependencyModule : Module
    {
        internal static IAssemblyRegistry? AssemblyRegistryTypedInstance;

        internal static object? AssemblyRegistryCustomInstance;
        internal static MethodInfo? GetAssembliesMethod;

        internal static ModuleOptions ModuleOptions = new();
        internal static ContainerBuilder? ModulesContainerBuilder;
        internal static readonly List<Action<ContainerBuilder>> RegisterModuleDependencyInstanceActions = new();

        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            if (
                TryGetAssemblies(
                    out Assembly[]? assemblies,
                    () => AssemblyRegistryTypedInstance?.GetAssemblies(),
                    () => GetAssembliesMethod?.Invoke(AssemblyRegistryCustomInstance, null)
                )
            )
            {
                RegisterAssemblyModules(builder, assemblies);
            }
        }

        private bool TryGetAssemblies([NotNullWhen(true)] out Assembly[]? assemblies, params Func<object?>[] assembliesSelectors)
        {
            assemblies = null;

            foreach (Func<object?> assembliesSelector in assembliesSelectors)
            {
                if (assembliesSelector.Invoke() is IEnumerable<Assembly> selectedAssemblies)
                {
                    assemblies = selectedAssemblies.Where(assembly => assembly != ThisAssembly).ToArray();
                    return true;
                }
            }

            return false;
        }

        [MemberNotNullWhen(true, nameof(ModulesContainerBuilder))]
        private static bool ShouldTreatModulesAsServices()
        {
            return ModuleOptions.TreatModulesAsServices;
        }

        private static void RegisterAssemblyModules(ContainerBuilder builder, Assembly[] assemblies)
        {
            if (!ShouldTreatModulesAsServices())
            {
                builder.RegisterAssemblyModules(assemblies);
                return;
            }

            RegisterModuleDependencies();

            RegisterModules(assemblies);

            using IContainer modulesContainer = ModulesContainerBuilder.Build();

            IEnumerable<IModule> modules = modulesContainer.Resolve<IEnumerable<IModule>>();
            IServiceCollection? services = modulesContainer.ResolveOptional<IServiceCollection>();

            builder.RegisterCallback(
                componentRegistryBuilder =>
                {
                    modules.ToList().ForEach(module => module.Configure(componentRegistryBuilder));

                    if (services is not null || ModuleOptions.OnModulesRegistered is not null)
                    {
                        new ServiceCollectionDependencyModule(services, ModuleOptions.OnModulesRegistered).Configure(componentRegistryBuilder);
                    }
                }
            );
        }

        private static void RegisterModuleDependencies()
        {
            if (!ShouldTreatModulesAsServices())
            {
                return;
            }

            RegisterModuleDependencyInstanceActions.ForEach(action => action.Invoke(ModulesContainerBuilder));
        }

        private static void RegisterModules(Assembly[] assemblies)
        {
            if (!ShouldTreatModulesAsServices())
            {
                return;
            }

            Type[] moduleTypes = GetModuleTypes(assemblies);

            foreach (Type moduleType in moduleTypes)
            {
                switch (ModuleOptions.DependencyInjectionOption)
                {
                    case ModuleDependencyInjectionOption.PropertyInjection:
                        ModulesContainerBuilder
                            .RegisterType(moduleType)
                            .As<IModule>()
                            .SingleInstance()
                            .PropertiesAutowired();
                        break;

                    case ModuleDependencyInjectionOption.ConstructorInjection:
                        ModulesContainerBuilder
                            .RegisterType(moduleType)
                            .As<IModule>()
                            .SingleInstance();
                        break;

                    case ModuleDependencyInjectionOption.NoInjection:
                    default:
                        break;
                }
            }
        }

        private static Type[] GetModuleTypes(IEnumerable<Assembly> assemblies)
        {
            return
                assemblies
                    .SelectMany(
                        assembly =>
                        assembly
                            .DefinedTypes
                            .Where(typeInfo => !typeInfo.IsAbstract && typeof(IModule).IsAssignableFrom(typeInfo.AsType()))
                            .Select(typeInfo => typeInfo.AsType())
                    )
                    .ToArray();
        }

        private sealed class ServiceCollectionDependencyModule : Module
        {
            private readonly IServiceCollection? services;
            private readonly Action<ContainerBuilder>? action;

            public ServiceCollectionDependencyModule(IServiceCollection? services, Action<ContainerBuilder>? action)
            {
                this.services = services;
                this.action = action;
            }

            protected override void Load(ContainerBuilder builder)
            {
                action?.Invoke(builder);

                if (services is not null)
                {
                    builder.Populate(services);
                }
            }
        }
    }
}
