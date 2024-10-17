// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;

using Autofac;
using Autofac.Core;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    /// <summary>
    /// Controls how discovered <see cref="Module"/> types are treated.
    /// </summary>
    public class ModuleOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleOptions"/> class.
        /// </summary>
        public ModuleOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleOptions"/> class with the specified <paramref name="options"/>.
        /// </summary>
        public ModuleOptions(ModuleOptions options)
        {
            TreatModulesAsServices = options.TreatModulesAsServices;
            DependencyInjectionOption = options.DependencyInjectionOption;
            OnModulesRegistered = options.OnModulesRegistered;
        }

        /// <summary>
        /// Gets or sets the flag which decides whether discovered <see cref="IModule"/> types
        /// should be treated as services. <see langword="false"/> by default.
        /// <para>
        /// When <see langword="true"/>,<br/>
        /// - modules will be registered in a separate, temporary container,<br/>
        /// - modules can have dependencies,<br/>
        /// - dependencies (e.g. IConfiguration, IHostEnvironment etc. ) can be registered into that container.
        /// </para>
        /// </summary>
        public bool TreatModulesAsServices { get; set; }

        /// <summary>
        /// Gets or sets the flag which decides how dependencies are injected into the <see cref="IModule"/> instances.
        /// </summary>
        public ModuleDependencyInjectionOption DependencyInjectionOption { get; set; }

        /// <summary>
        /// Gets or sets an action that will be executed after all discovered <see cref="IModule"/> instances are registered.
        /// </summary>
        public Action<ContainerBuilder>? OnModulesRegistered { get; set; }
    }
}
