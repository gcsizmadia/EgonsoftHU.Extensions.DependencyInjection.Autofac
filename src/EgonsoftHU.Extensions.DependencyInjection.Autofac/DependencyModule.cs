// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using Autofac;

using Module = Autofac.Module;

namespace EgonsoftHU.Extensions.DependencyInjection.Autofac
{
    /// <summary>
    /// Registers all other dependency modules that are derived from <see cref="Module"/> type.
    /// </summary>
    public class DependencyModule : Module
    {
        internal static IAssemblyRegistry? AssemblyRegistryTypedInstance;

        internal static object? AssemblyRegistryCustomInstance;
        internal static MethodInfo? GetAssembliesMethod;

        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            Assembly[] assemblies;

            if (AssemblyRegistryTypedInstance?.GetAssemblies() is Assembly[] assemblies1)
            {
                assemblies = ExcludeThisAssembly(assemblies1);
            }
            else if (GetAssembliesMethod?.Invoke(AssemblyRegistryCustomInstance, null) is IEnumerable<Assembly> assemblies2)
            {
                assemblies = ExcludeThisAssembly(assemblies2);
            }
            else
            {
                return;
            }

            builder.RegisterAssemblyModules(assemblies);
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
    }
}
