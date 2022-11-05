// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    /// <summary>
    /// Controls how dependencies are injected into <see cref="Module"/> instances.
    /// </summary>
    public enum ModuleDependencyInjectionOption
    {
        /// <summary>
        /// No dependency will be injected into modules.
        /// </summary>
        NoInjection,

        /// <summary>
        /// Any properties whose types are registered in the separate, temporary container will be wired to instances of the appropriate service.
        /// </summary>
        PropertyInjection,

        /// <summary>
        /// Dependencies will be injected through the module's constructor.
        /// </summary>
        ConstructorInjection
    }
}
