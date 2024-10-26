﻿// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;

using Autofac;
using Autofac.Core;

using EgonsoftHU.Extensions.Bcl;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    /// <summary>
    /// Controls module dependencies.
    /// </summary>
    public static class ModuleContainerBuilderExtensions
    {
        /// <summary>
        /// Treats discovered <see cref="IModule"/> types as services, i.e.,<br/>
        /// - modules will be registered in a separate, temporary container,<br/>
        /// - modules can have dependencies,<br/>
        /// - dependencies (e.g., IConfiguration, IHostEnvironment etc.) can be registered into that container,<br/>
        /// - dependencies are injected using property injection.
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/> to add the services to.</param>
        /// <remarks>
        /// Please note:
        /// <br/>- <paramref name="builder"/> is not affected by this method call at all.
        /// </remarks>
        /// <returns>The <see cref="ContainerBuilder"/> so that additional calls can be chained.</returns>
        public static ContainerBuilder TreatModulesAsServices(this ContainerBuilder builder)
        {
            return
                builder.ConfigureModuleOptions(
                    options =>
                    {
                        options.TreatModulesAsServices = true;
                        options.DependencyInjectionOption = ModuleDependencyInjectionOption.PropertyInjection;
                    }
                );
        }

        /// <summary>
        /// Configures <see cref="ModuleOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/> to add the services to.</param>
        /// <param name="setupAction">The configure action.</param>
        /// <remarks>
        /// Please note:
        /// <br/>- <paramref name="builder"/> is not affected by this method call at all.
        /// </remarks>
        /// <returns>The <see cref="ContainerBuilder"/> so that additional calls can be chained.</returns>
        public static ContainerBuilder ConfigureModuleOptions(this ContainerBuilder builder, Action<ModuleOptions> setupAction)
        {
            builder.ThrowIfNull();
            setupAction.ThrowIfNull();

            var options = new ModuleOptions(DependencyModule.ModuleOptions);
            setupAction.Invoke(options);

            bool isCorrectSetup =
                options.TreatModulesAsServices
                ^
                options.DependencyInjectionOption == ModuleDependencyInjectionOption.NoInjection;

            if (!isCorrectSetup)
            {
                var ex =
                    new ArgumentException(
                        "Invalid module options configuration. See Data[\"ErrorReason\"] for details.",
                        nameof(setupAction)
                    );

                ex.Data[nameof(ModuleOptions.TreatModulesAsServices)] = options.TreatModulesAsServices;
                ex.Data[nameof(ModuleOptions.DependencyInjectionOption)] = options.DependencyInjectionOption;
                ex.Data["ErrorReason"] =
                    options.TreatModulesAsServices
                        ? "When TreatModulesAsServices is true then DependencyInjectionOption cannot be NoInjection."
                        : "When TreatModulesAsServices is false then DependencyInjectionOption must be NoInjection.";

                throw ex;
            }

            DependencyModule.ModuleOptions = options;

            if (options.TreatModulesAsServices)
            {
                DependencyModule.ModulesContainerBuilder ??= new();
            }

            return builder;
        }

        /// <summary>
        /// Register an instance of the <typeparamref name="T"/> service type to the separate, temporary container.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <param name="builder">The <see cref="ContainerBuilder"/> to add the services to.</param>
        /// <param name="instance">An instance of the <typeparamref name="T"/> service type.</param>
        /// <remarks>
        /// Please note:
        /// <br/>- <paramref name="instance"/> is registered in a separate, temporary container.
        /// <br/>- <paramref name="builder"/> is not affected by this method call at all.
        /// </remarks>
        /// <returns>The <see cref="ContainerBuilder"/> so that additional calls can be chained.</returns>
        public static ContainerBuilder RegisterModuleDependencyInstance<T>(this ContainerBuilder builder, T instance)
            where T : class
        {
            builder.ThrowIfNull();
            instance.ThrowIfNull();

            bool treatModulesAsServices = DependencyModule.ModuleOptions.TreatModulesAsServices;

            if (!treatModulesAsServices)
            {
                var ex =
                    new InvalidOperationException(
                        "Register module dependency instance failed. See Data[\"ErrorReason\"] for details."
                    );

                ex.Data[nameof(ModuleOptions.TreatModulesAsServices)] = treatModulesAsServices;
                ex.Data["ErrorReason"] = "TreatModulesAsServices must be true.";

                throw ex;
            }

            DependencyModule.RegisterModuleDependencyInstanceActions.Add(
                containerBuilder => containerBuilder.RegisterInstance(instance).ExternallyOwned()
            );

            return builder;
        }
    }
}
