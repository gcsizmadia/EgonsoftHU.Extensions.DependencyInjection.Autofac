// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using Autofac;
using Autofac.Core;

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
                builder.RegisterAssemblyModules(assemblies);
            }
        }

        private bool TryGetAssemblies([NotNullWhen(true)] out Assembly[]? assemblies, params Func<object?>[] assembliesSelectors)
        {
            assemblies = null;

            foreach (Func<object?> assembliesSelector in assembliesSelectors)
            {
                if (assembliesSelector.Invoke() is IEnumerable<Assembly> selectedAssemblies)
                {
                    assemblies = ExcludeThisAssembly(selectedAssemblies);
                    return true;
                }
            }

            return false;
        }

        private Assembly[] ExcludeThisAssembly(IEnumerable<Assembly> assemblies)
        {
            return assemblies.Where(assembly => assembly != ThisAssembly).ToArray();
        }

        [MemberNotNullWhen(true, nameof(ModulesContainerBuilder))]
        private static bool ShouldTreatModulesAsServices()
        {
            return ModuleOptions.TreatModulesAsServices;
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
    }
}
