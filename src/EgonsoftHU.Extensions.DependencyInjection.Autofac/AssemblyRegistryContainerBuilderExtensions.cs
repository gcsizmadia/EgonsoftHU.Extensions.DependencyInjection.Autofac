// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;

using EgonsoftHU.Extensions.Bcl;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    /// <summary>
    /// Controls assembly registry configuration.
    /// </summary>
    public static class AssemblyRegistryContainerBuilderExtensions
    {
        private static bool isConfigured = false;

        /// <summary>
        /// Use a custom assembly registry.
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/> to add the services to.</param>
        /// <param name="assemblyRegistry">The assembly registry instance.</param>
        /// <returns>The <see cref="ContainerBuilder"/> so that additional calls can be chained.</returns>
        public static ContainerBuilder UseAssemblyRegistry(this ContainerBuilder builder, object assemblyRegistry)
        {
            SetConfiguredOrThrow();

            builder.ThrowIfNull();
            assemblyRegistry.ThrowIfNull();

            DependencyModule.AssemblyRegistryCustomInstance = assemblyRegistry;

            TypeInfo typeInfo = assemblyRegistry.GetType().GetTypeInfo();

            DependencyModule.GetAssembliesMethod =
                typeInfo
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .SingleOrDefault(
                        methodInfo =>
                        String.Equals(nameof(IAssemblyRegistry.GetAssemblies), methodInfo.Name, StringComparison.OrdinalIgnoreCase)
                        &&
                        methodInfo.GetParameters().Length == 0
                        &&
                        typeof(IEnumerable<Assembly>).IsAssignableFrom(methodInfo.ReturnType)
                    );

            if (DependencyModule.GetAssembliesMethod is null)
            {
                var ex = new ArgumentException($"Expected method not found in type. Type=[{typeInfo.FullName}]", nameof(assemblyRegistry));
                ex.Data["ExpectedMethod.IsPublic"] = true;
                ex.Data["ExpectedMethod.IsStatic"] = false;
                ex.Data["ExpectedMethod.ReturnType"] = typeof(IEnumerable<Assembly>);
                ex.Data["ExpectedMethod.Name"] = nameof(IAssemblyRegistry.GetAssemblies);
                ex.Data["ExpectedMethod.GetParameter().Length"] = 0;

                throw ex;
            }

            return builder;
        }

        /// <summary>
        /// Use a custom assembly registry.
        /// </summary>
        /// <typeparam name="TAssemblyRegistry">The type of the assembly registry. A parameterless ctor is required.</typeparam>
        /// <param name="builder">The <see cref="ContainerBuilder"/> to add the services to.</param>
        /// <returns>The <see cref="ContainerBuilder"/> so that additional calls can be chained.</returns>
        public static ContainerBuilder UseAssemblyRegistry<TAssemblyRegistry>(this ContainerBuilder builder)
            where TAssemblyRegistry : IAssemblyRegistry, new()
        {
            SetConfiguredOrThrow();

            builder.ThrowIfNull();

            DependencyModule.AssemblyRegistryTypedInstance = new TAssemblyRegistry();

            return builder;
        }

        /// <summary>
        /// Use a custom assembly registry.
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/> to add the services to.</param>
        /// <param name="assemblyRegistry">The assembly registry instance.</param>
        /// <returns>The <see cref="ContainerBuilder"/> so that additional calls can be chained.</returns>
        public static ContainerBuilder UseAssemblyRegistry(this ContainerBuilder builder, IAssemblyRegistry assemblyRegistry)
        {
            SetConfiguredOrThrow();

            builder.ThrowIfNull();
            assemblyRegistry.ThrowIfNull();

            DependencyModule.AssemblyRegistryTypedInstance = assemblyRegistry;

            return builder;
        }

        /// <summary>
        /// Use the default assembly registry that uses assembly file name prefixes the search for assemblies to register.
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/> to add the services to.</param>
        /// <param name="assemblyFileNamePrefixes">The prefixes for assembly file names.</param>
        /// <remarks>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Calling
        /// <br/><c>AssemblyRegistryConfiguration.UseDefaultAssemblyRegistry("MyCompany");</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>The <see cref="ContainerBuilder"/> so that additional calls can be chained.</returns>
        public static ContainerBuilder UseDefaultAssemblyRegistry(this ContainerBuilder builder, params string[] assemblyFileNamePrefixes)
        {
            SetConfiguredOrThrow();

            builder.ThrowIfNull();

            DependencyModule.AssemblyRegistryTypedInstance = DefaultAssemblyRegistry.Initialize(assemblyFileNamePrefixes);

            return builder;
        }

        private static void SetConfiguredOrThrow()
        {
            if (isConfigured)
            {
                throw new InvalidOperationException("Assembly Registry already configured");
            }

            isConfigured = true;
        }
    }
}
