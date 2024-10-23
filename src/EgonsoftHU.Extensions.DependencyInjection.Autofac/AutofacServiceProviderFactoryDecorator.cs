// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;

using EgonsoftHU.Extensions.Bcl;

using Microsoft.Extensions.DependencyInjection;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    /// <summary>
    /// A factory for creating a <see cref="ContainerBuilder"/> and an <see cref="IServiceProvider"/>.
    /// </summary>
    public sealed class AutofacServiceProviderFactoryDecorator : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly AutofacServiceProviderFactory autofacServiceProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="assemblyFileNamePrefix">The prefix for assembly file names.</param>
        /// <remarks>
        /// The following defaults are applied:
        /// <list type="bullet">
        /// <item><see cref="DefaultAssemblyRegistry"/> is used with the specified <paramref name="assemblyFileNamePrefix"/>.</item>
        /// <item>Types derived from the <see cref="Module"/> type will be treated as services, hence dependencies can be injected into them.</item>
        /// <item>
        /// <see cref="DependencyModule"/> will be registered that will register all other dependency modules that are derived from
        /// <see cref="Module"/> type and can be found in the <see cref="DefaultAssemblyRegistry"/>.
        /// </item>
        /// </list>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Note: <c>EgonsoftHU</c> will be automatically added to the prefix collection.
        /// </para>
        /// <para>
        /// Calling
        /// <br/><c>AutofacServiceProviderFactoryDecorator.CreateDefault("MyCompany");</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c> and <c>EgonsoftHU.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.</returns>
        public static AutofacServiceProviderFactoryDecorator CreateDefault(string assemblyFileNamePrefix)
        {
            return CreateDefault(GetAssemblyFileNamePrefixes(assemblyFileNamePrefix));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="assemblyFileNamePrefixes">The prefixes for assembly file names.</param>
        /// <remarks>
        /// The following defaults are applied:
        /// <list type="bullet">
        /// <item><see cref="DefaultAssemblyRegistry"/> is used with the specified <paramref name="assemblyFileNamePrefixes"/>.</item>
        /// <item>Types derived from the <see cref="Module"/> type will be treated as services, hence dependencies can be injected into them.</item>
        /// <item>
        /// <see cref="DependencyModule"/> will be registered that will register all other dependency modules that are derived from
        /// <see cref="Module"/> type and can be found in the <see cref="DefaultAssemblyRegistry"/>.
        /// </item>
        /// </list>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Note: <c>EgonsoftHU</c> will be automatically added to the <paramref name="assemblyFileNamePrefixes"/> collection.
        /// </para>
        /// <para>
        /// Calling
        /// <br/><c>AutofacServiceProviderFactoryDecorator.CreateDefault(new[] { "MyCompany" });</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c> and <c>EgonsoftHU.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.</returns>
        public static AutofacServiceProviderFactoryDecorator CreateDefault(IReadOnlyCollection<string> assemblyFileNamePrefixes)
        {
            return CreateDefaultCore(assemblyFileNamePrefixes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.
        /// </summary>
        /// <typeparam name="TDependency">The type of the module dependency.</typeparam>
        /// <param name="dependency">The module dependency instance.</param>
        /// <param name="assemblyFileNamePrefix">The prefix for assembly file names.</param>
        /// <remarks>
        /// The following defaults are applied:
        /// <list type="bullet">
        /// <item><see cref="DefaultAssemblyRegistry"/> is used with the specified <paramref name="assemblyFileNamePrefix"/>.</item>
        /// <item>Types derived from the <see cref="Module"/> type will be treated as services, hence dependencies can be injected into them.</item>
        /// <item>The <paramref name="dependency"/> of type <typeparamref name="TDependency"/> will be injected into the dependency modules.</item>
        /// <item>
        /// <see cref="DependencyModule"/> will be registered that will register all other dependency modules that are derived from
        /// <see cref="Module"/> type and can be found in the <see cref="DefaultAssemblyRegistry"/>.
        /// </item>
        /// </list>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Note: <c>EgonsoftHU</c> will be automatically added to the prefix collection.
        /// </para>
        /// <para>
        /// Calling
        /// <br/><c>AutofacServiceProviderFactoryDecorator.CreateDefault("MyCompany");</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c> and <c>EgonsoftHU.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.</returns>
        public static AutofacServiceProviderFactoryDecorator CreateDefault<TDependency>(string assemblyFileNamePrefix, TDependency dependency)
            where TDependency : class
        {
            return CreateDefault(GetAssemblyFileNamePrefixes(assemblyFileNamePrefix), dependency);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.
        /// </summary>
        /// <typeparam name="TDependency">The type of the module dependency.</typeparam>
        /// <param name="dependency">The module dependency instance.</param>
        /// <param name="assemblyFileNamePrefixes">The prefixes for assembly file names.</param>
        /// <remarks>
        /// The following defaults are applied:
        /// <list type="bullet">
        /// <item><see cref="DefaultAssemblyRegistry"/> is used with the specified <paramref name="assemblyFileNamePrefixes"/>.</item>
        /// <item>Types derived from the <see cref="Module"/> type will be treated as services, hence dependencies can be injected into them.</item>
        /// <item>The <paramref name="dependency"/> of type <typeparamref name="TDependency"/> will be injected into the dependency modules.</item>
        /// <item>
        /// <see cref="DependencyModule"/> will be registered that will register all other dependency modules that are derived from
        /// <see cref="Module"/> type and can be found in the <see cref="DefaultAssemblyRegistry"/>.
        /// </item>
        /// </list>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Note: <c>EgonsoftHU</c> will be automatically added to the <paramref name="assemblyFileNamePrefixes"/> collection.
        /// </para>
        /// <para>
        /// Calling
        /// <br/><c>AutofacServiceProviderFactoryDecorator.CreateDefault(new[] { "MyCompany" });</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c> and <c>EgonsoftHU.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.</returns>
        public static AutofacServiceProviderFactoryDecorator CreateDefault<TDependency>(
            IReadOnlyCollection<string> assemblyFileNamePrefixes,
            TDependency dependency
        )
            where TDependency : class
        {
            return CreateDefaultCore(assemblyFileNamePrefixes, new ConfigureContainerActionsBuilder() { dependency }.Build());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.
        /// </summary>
        /// <typeparam name="TDependency1">The type of the first module dependency.</typeparam>
        /// <typeparam name="TDependency2">The type of the second module dependency.</typeparam>
        /// <param name="dependency1">The first module dependency instance.</param>
        /// <param name="dependency2">The second module dependency instance.</param>
        /// <param name="assemblyFileNamePrefix">The prefix for assembly file names.</param>
        /// <remarks>
        /// The following defaults are applied:
        /// <list type="bullet">
        /// <item><see cref="DefaultAssemblyRegistry"/> is used with the specified <paramref name="assemblyFileNamePrefix"/>.</item>
        /// <item>Types derived from the <see cref="Module"/> type will be treated as services, hence dependencies can be injected into them.</item>
        /// <item>The <paramref name="dependency1"/> of type <typeparamref name="TDependency1"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency2"/> of type <typeparamref name="TDependency2"/> will be injected into the dependency modules.</item>
        /// <item>
        /// <see cref="DependencyModule"/> will be registered that will register all other dependency modules that are derived from
        /// <see cref="Module"/> type and can be found in the <see cref="DefaultAssemblyRegistry"/>.
        /// </item>
        /// </list>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Note: <c>EgonsoftHU</c> will be automatically added to the prefix collection.
        /// </para>
        /// <para>
        /// Calling
        /// <br/><c>AutofacServiceProviderFactoryDecorator.CreateDefault("MyCompany");</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c> and <c>EgonsoftHU.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.</returns>
        public static AutofacServiceProviderFactoryDecorator CreateDefault<TDependency1, TDependency2>(
            string assemblyFileNamePrefix,
            TDependency1 dependency1,
            TDependency2 dependency2
        )
            where TDependency1 : class
            where TDependency2 : class
        {
            return CreateDefault(GetAssemblyFileNamePrefixes(assemblyFileNamePrefix), dependency1, dependency2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.
        /// </summary>
        /// <typeparam name="TDependency1">The type of the first module dependency.</typeparam>
        /// <typeparam name="TDependency2">The type of the second module dependency.</typeparam>
        /// <param name="dependency1">The first module dependency instance.</param>
        /// <param name="dependency2">The second module dependency instance.</param>
        /// <param name="assemblyFileNamePrefixes">The prefixes for assembly file names.</param>
        /// <remarks>
        /// The following defaults are applied:
        /// <list type="bullet">
        /// <item><see cref="DefaultAssemblyRegistry"/> is used with the specified <paramref name="assemblyFileNamePrefixes"/>.</item>
        /// <item>Types derived from the <see cref="Module"/> type will be treated as services, hence dependencies can be injected into them.</item>
        /// <item>The <paramref name="dependency1"/> of type <typeparamref name="TDependency1"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency2"/> of type <typeparamref name="TDependency2"/> will be injected into the dependency modules.</item>
        /// <item>
        /// <see cref="DependencyModule"/> will be registered that will register all other dependency modules that are derived from
        /// <see cref="Module"/> type and can be found in the <see cref="DefaultAssemblyRegistry"/>.
        /// </item>
        /// </list>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Note: <c>EgonsoftHU</c> will be automatically added to the <paramref name="assemblyFileNamePrefixes"/> collection.
        /// </para>
        /// <para>
        /// Calling
        /// <br/><c>AutofacServiceProviderFactoryDecorator.CreateDefault(new[] { "MyCompany" });</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c> and <c>EgonsoftHU.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.</returns>
        public static AutofacServiceProviderFactoryDecorator CreateDefault<TDependency1, TDependency2>(
            IReadOnlyCollection<string> assemblyFileNamePrefixes,
            TDependency1 dependency1,
            TDependency2 dependency2
        )
            where TDependency1 : class
            where TDependency2 : class
        {
            return CreateDefaultCore(assemblyFileNamePrefixes, new ConfigureContainerActionsBuilder() { dependency1, dependency2 }.Build());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.
        /// </summary>
        /// <typeparam name="TDependency1">The type of the first module dependency.</typeparam>
        /// <typeparam name="TDependency2">The type of the second module dependency.</typeparam>
        /// <typeparam name="TDependency3">The type of the third module dependency.</typeparam>
        /// <param name="dependency1">The first module dependency instance.</param>
        /// <param name="dependency2">The second module dependency instance.</param>
        /// <param name="dependency3">The third module dependency instance.</param>
        /// <param name="assemblyFileNamePrefix">The prefix for assembly file names.</param>
        /// <remarks>
        /// The following defaults are applied:
        /// <list type="bullet">
        /// <item><see cref="DefaultAssemblyRegistry"/> is used with the specified <paramref name="assemblyFileNamePrefix"/>.</item>
        /// <item>Types derived from the <see cref="Module"/> type will be treated as services, hence dependencies can be injected into them.</item>
        /// <item>The <paramref name="dependency1"/> of type <typeparamref name="TDependency1"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency2"/> of type <typeparamref name="TDependency2"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency3"/> of type <typeparamref name="TDependency3"/> will be injected into the dependency modules.</item>
        /// <item>
        /// <see cref="DependencyModule"/> will be registered that will register all other dependency modules that are derived from
        /// <see cref="Module"/> type and can be found in the <see cref="DefaultAssemblyRegistry"/>.
        /// </item>
        /// </list>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Note: <c>EgonsoftHU</c> will be automatically added to the prefix collection.
        /// </para>
        /// <para>
        /// Calling
        /// <br/><c>AutofacServiceProviderFactoryDecorator.CreateDefault("MyCompany");</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c> and <c>EgonsoftHU.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.</returns>
        public static AutofacServiceProviderFactoryDecorator CreateDefault<TDependency1, TDependency2, TDependency3>(
            string assemblyFileNamePrefix,
            TDependency1 dependency1,
            TDependency2 dependency2,
            TDependency3 dependency3
        )
            where TDependency1 : class
            where TDependency2 : class
            where TDependency3 : class
        {
            return CreateDefault(GetAssemblyFileNamePrefixes(assemblyFileNamePrefix), dependency1, dependency2, dependency3);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.
        /// </summary>
        /// <typeparam name="TDependency1">The type of the first module dependency.</typeparam>
        /// <typeparam name="TDependency2">The type of the second module dependency.</typeparam>
        /// <typeparam name="TDependency3">The type of the third module dependency.</typeparam>
        /// <param name="dependency1">The first module dependency instance.</param>
        /// <param name="dependency2">The second module dependency instance.</param>
        /// <param name="dependency3">The third module dependency instance.</param>
        /// <param name="assemblyFileNamePrefixes">The prefixes for assembly file names.</param>
        /// <remarks>
        /// The following defaults are applied:
        /// <list type="bullet">
        /// <item><see cref="DefaultAssemblyRegistry"/> is used with the specified <paramref name="assemblyFileNamePrefixes"/>.</item>
        /// <item>Types derived from the <see cref="Module"/> type will be treated as services, hence dependencies can be injected into them.</item>
        /// <item>The <paramref name="dependency1"/> of type <typeparamref name="TDependency1"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency2"/> of type <typeparamref name="TDependency2"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency3"/> of type <typeparamref name="TDependency3"/> will be injected into the dependency modules.</item>
        /// <item>
        /// <see cref="DependencyModule"/> will be registered that will register all other dependency modules that are derived from
        /// <see cref="Module"/> type and can be found in the <see cref="DefaultAssemblyRegistry"/>.
        /// </item>
        /// </list>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Note: <c>EgonsoftHU</c> will be automatically added to the <paramref name="assemblyFileNamePrefixes"/> collection.
        /// </para>
        /// <para>
        /// Calling
        /// <br/><c>AutofacServiceProviderFactoryDecorator.CreateDefault(new[] { "MyCompany" });</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c> and <c>EgonsoftHU.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.</returns>
        public static AutofacServiceProviderFactoryDecorator CreateDefault<TDependency1, TDependency2, TDependency3>(
            IReadOnlyCollection<string> assemblyFileNamePrefixes,
            TDependency1 dependency1,
            TDependency2 dependency2,
            TDependency3 dependency3
        )
            where TDependency1 : class
            where TDependency2 : class
            where TDependency3 : class
        {
            return
                CreateDefaultCore(
                    assemblyFileNamePrefixes,
                    new ConfigureContainerActionsBuilder() { dependency1, dependency2, dependency3 }.Build()
                );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.
        /// </summary>
        /// <typeparam name="TDependency1">The type of the first module dependency.</typeparam>
        /// <typeparam name="TDependency2">The type of the second module dependency.</typeparam>
        /// <typeparam name="TDependency3">The type of the third module dependency.</typeparam>
        /// <typeparam name="TDependency4">The type of the fourth module dependency.</typeparam>
        /// <param name="dependency1">The first module dependency instance.</param>
        /// <param name="dependency2">The second module dependency instance.</param>
        /// <param name="dependency3">The third module dependency instance.</param>
        /// <param name="dependency4">The fourth module dependency instance.</param>
        /// <param name="assemblyFileNamePrefix">The prefix for assembly file names.</param>
        /// <remarks>
        /// The following defaults are applied:
        /// <list type="bullet">
        /// <item><see cref="DefaultAssemblyRegistry"/> is used with the specified <paramref name="assemblyFileNamePrefix"/>.</item>
        /// <item>Types derived from the <see cref="Module"/> type will be treated as services, hence dependencies can be injected into them.</item>
        /// <item>The <paramref name="dependency1"/> of type <typeparamref name="TDependency1"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency2"/> of type <typeparamref name="TDependency2"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency3"/> of type <typeparamref name="TDependency3"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency4"/> of type <typeparamref name="TDependency4"/> will be injected into the dependency modules.</item>
        /// <item>
        /// <see cref="DependencyModule"/> will be registered that will register all other dependency modules that are derived from
        /// <see cref="Module"/> type and can be found in the <see cref="DefaultAssemblyRegistry"/>.
        /// </item>
        /// </list>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Note: <c>EgonsoftHU</c> will be automatically added to the prefix collection.
        /// </para>
        /// <para>
        /// Calling
        /// <br/><c>AutofacServiceProviderFactoryDecorator.CreateDefault("MyCompany");</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c> and <c>EgonsoftHU.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.</returns>
        public static AutofacServiceProviderFactoryDecorator CreateDefault<TDependency1, TDependency2, TDependency3, TDependency4>(
            string assemblyFileNamePrefix,
            TDependency1 dependency1,
            TDependency2 dependency2,
            TDependency3 dependency3,
            TDependency4 dependency4
        )
            where TDependency1 : class
            where TDependency2 : class
            where TDependency3 : class
            where TDependency4 : class
        {
            return CreateDefault(GetAssemblyFileNamePrefixes(assemblyFileNamePrefix), dependency1, dependency2, dependency3, dependency4);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.
        /// </summary>
        /// <typeparam name="TDependency1">The type of the first module dependency.</typeparam>
        /// <typeparam name="TDependency2">The type of the second module dependency.</typeparam>
        /// <typeparam name="TDependency3">The type of the third module dependency.</typeparam>
        /// <typeparam name="TDependency4">The type of the fourth module dependency.</typeparam>
        /// <param name="dependency1">The first module dependency instance.</param>
        /// <param name="dependency2">The second module dependency instance.</param>
        /// <param name="dependency3">The third module dependency instance.</param>
        /// <param name="dependency4">The fourth module dependency instance.</param>
        /// <param name="assemblyFileNamePrefixes">The prefixes for assembly file names.</param>
        /// <remarks>
        /// The following defaults are applied:
        /// <list type="bullet">
        /// <item><see cref="DefaultAssemblyRegistry"/> is used with the specified <paramref name="assemblyFileNamePrefixes"/>.</item>
        /// <item>Types derived from the <see cref="Module"/> type will be treated as services, hence dependencies can be injected into them.</item>
        /// <item>The <paramref name="dependency1"/> of type <typeparamref name="TDependency1"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency2"/> of type <typeparamref name="TDependency2"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency3"/> of type <typeparamref name="TDependency3"/> will be injected into the dependency modules.</item>
        /// <item>The <paramref name="dependency4"/> of type <typeparamref name="TDependency4"/> will be injected into the dependency modules.</item>
        /// <item>
        /// <see cref="DependencyModule"/> will be registered that will register all other dependency modules that are derived from
        /// <see cref="Module"/> type and can be found in the <see cref="DefaultAssemblyRegistry"/>.
        /// </item>
        /// </list>
        /// The file name pattern for searching assembly files will be the following: <c>[prefix].*.dll</c>
        /// <para>
        /// Note: <c>EgonsoftHU</c> will be automatically added to the <paramref name="assemblyFileNamePrefixes"/> collection.
        /// </para>
        /// <para>
        /// Calling
        /// <br/><c>AutofacServiceProviderFactoryDecorator.CreateDefault(new[] { "MyCompany" });</c>
        /// <br/><br/>will search for assemblies with this file name pattern:
        /// <br/><c>MyCompany.*.dll</c> and <c>EgonsoftHU.*.dll</c>
        /// </para>
        /// </remarks>
        /// <returns>a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class with preconfigured defaults.</returns>
        public static AutofacServiceProviderFactoryDecorator CreateDefault<TDependency1, TDependency2, TDependency3, TDependency4>(
            IReadOnlyCollection<string> assemblyFileNamePrefixes,
            TDependency1 dependency1,
            TDependency2 dependency2,
            TDependency3 dependency3,
            TDependency4 dependency4
        )
            where TDependency1 : class
            where TDependency2 : class
            where TDependency3 : class
            where TDependency4 : class
        {
            return
                CreateDefaultCore(
                    assemblyFileNamePrefixes,
                    new ConfigureContainerActionsBuilder() { dependency1, dependency2, dependency3, dependency4 }.Build()
                );
        }

        private static AutofacServiceProviderFactoryDecorator CreateDefaultCore(
            IReadOnlyCollection<string> assemblyFileNamePrefixes,
            IReadOnlyCollection<Action<ContainerBuilder>>? configureContainerActions = null
        )
        {
            return new(
                GetConfigurationAction(
                    assemblyFileNamePrefixes,
                    configureContainerActions ?? Array.Empty<Action<ContainerBuilder>>()
                )
            );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class.
        /// </summary>
        /// <param name="containerBuildOptions">The container options to use when building the container.</param>
        /// <param name="configurationAction">
        /// Action on a <see cref="ContainerBuilder"/> that adds component registrations to the container.
        /// </param>
        public AutofacServiceProviderFactoryDecorator(
            ContainerBuildOptions containerBuildOptions,
            Action<ContainerBuilder>? configurationAction = null
        )
        {
            autofacServiceProviderFactory = new(containerBuildOptions, configurationAction);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactoryDecorator"/> class.
        /// </summary>
        /// <param name="configurationAction">
        /// Action on a <see cref="ContainerBuilder"/> that adds component registrations to the container.
        /// </param>
        public AutofacServiceProviderFactoryDecorator(Action<ContainerBuilder>? configurationAction = null)
            : this(default, configurationAction)
        {
        }

        /// <inheritdoc/>
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            DependencyModule.AlreadyPopulatedServices.AddRange(services);

            return autofacServiceProviderFactory.CreateBuilder(services);
        }

        /// <inheritdoc/>
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            containerBuilder.ThrowIfNull();

            return autofacServiceProviderFactory.CreateServiceProvider(containerBuilder);
        }

        private static IReadOnlyCollection<string> GetAssemblyFileNamePrefixes(string assemblyFileNamePrefix)
        {
            return
#if LANGVERSION12_0_OR_GREATER
                [assemblyFileNamePrefix]
#else
                new[] { assemblyFileNamePrefix }
#endif
                ;
        }

        private static Action<ContainerBuilder> GetConfigurationAction(
            IReadOnlyCollection<string> assemblyFileNamePrefixes,
            IReadOnlyCollection<Action<ContainerBuilder>> configureContainerActions
        )
        {
            return containerBuilder =>
            {
                containerBuilder
                    .UseDefaultAssemblyRegistry(assemblyFileNamePrefixes.ToArray())
                    .TreatModulesAsServices();

                foreach (Action<ContainerBuilder> configureContainerAction in configureContainerActions)
                {
                    configureContainerAction.Invoke(containerBuilder);
                }

                containerBuilder.RegisterModule<DependencyModule>();
            };
        }
    }
}
