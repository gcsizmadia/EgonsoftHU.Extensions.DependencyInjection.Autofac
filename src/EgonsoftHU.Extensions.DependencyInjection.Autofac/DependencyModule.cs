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

using EgonsoftHU.Extensions.Bcl;

using Microsoft.Extensions.DependencyInjection;

using Module = Autofac.Module;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    /// <summary>
    /// Registers all other dependency modules that are derived from <see cref="Module"/> type.
    /// </summary>
    public class DependencyModule : Module
    {
        internal static IAssemblyRegistry? AssemblyRegistryTypedInstance { private get; set; }

        internal static object? AssemblyRegistryCustomInstance { private get; set; }

        internal static MethodInfo? GetAssembliesMethod { get; set; }

        internal static ModuleOptions ModuleOptions { get; set; } = new();

        internal static ContainerBuilder? ModulesContainerBuilder { get; set; }

        internal static readonly List<Action<ContainerBuilder>> RegisterModuleDependencyInstanceActions =
#if LANGVERSION12_0_OR_GREATER
            []
#else
            new()
#endif
            ;

        internal static IServiceCollection AlreadyPopulatedServices { get; set; } = new ServiceCollection();

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
            IServiceCollection services = modulesContainer.Resolve<IServiceCollection>();

            builder.RegisterCallback(
                componentRegistryBuilder =>
                {
                    modules.ToList().ForEach(module => module.Configure(componentRegistryBuilder));

                    if (typeof(SharedServiceCollection) == services.GetType())
                    {
                        var servicesAddedByModules = new ServiceCollection();
                        servicesAddedByModules.AddRange(services.Except(AlreadyPopulatedServices));

                        services = servicesAddedByModules;
                    }

                    new ServiceCollectionDependencyModule(services, ModuleOptions.OnModulesRegistered)
                        .Configure(componentRegistryBuilder);
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

            var serviceCollection = new SharedServiceCollection();
            serviceCollection.AddRange(AlreadyPopulatedServices);

            ModulesContainerBuilder
                .RegisterInstance<IServiceCollection>(serviceCollection)
                .IfNotRegistered(typeof(IServiceCollection));
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
            private readonly IServiceCollection services;
            private readonly Action<ContainerBuilder>? action;

            public ServiceCollectionDependencyModule(IServiceCollection services, Action<ContainerBuilder>? action)
            {
                this.services = services;
                this.action = action;
            }

            protected override void Load(ContainerBuilder builder)
            {
                action?.Invoke(builder);

                builder.Populate(services);
            }
        }
    }
}
