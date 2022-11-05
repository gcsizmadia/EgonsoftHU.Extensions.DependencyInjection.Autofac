// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    /// <summary>
    /// Controls how discovered <see cref="Module"/> types are treated.
    /// </summary>
    public class ModuleOptions
    {
        /// <summary>
        /// Gets or sets the flag which decides whether discovered <see cref="Module"/> types
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
        /// Gets or sets the flag which decides how dependencies are injected into the <see cref="Module"/> instances.
        /// </summary>
        public ModuleDependencyInjectionOption DependencyInjectionOption { get; set; }
    }
}
