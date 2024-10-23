// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using EgonsoftHU.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Company.Product.Net8.WebApi
{
    internal static class IHostBuilderExtensions
    {
        internal static IHostBuilder UseAutofac(this IHostBuilder hostBuilder, EgonsoftHUDependenyInjectionSetupOption option)
        {
            return option switch
            {
                EgonsoftHUDependenyInjectionSetupOption.UseDecoratorWithFactoryMethod =>
                    hostBuilder.UseDecoratorWithFactoryMethod(),
                EgonsoftHUDependenyInjectionSetupOption.UseDecoratorWithConfigurationAction =>
                    hostBuilder.UseDecoratorWithConfigurationAction(),
                EgonsoftHUDependenyInjectionSetupOption.UseDecorator =>
                    hostBuilder.UseDecorator(),
                EgonsoftHUDependenyInjectionSetupOption.UseV3CompatibilityMode =>
                    hostBuilder.UseV3CompatibilityMode(),
                _ =>
                    throw new ArgumentOutOfRangeException(nameof(option))
            };
        }

        /// <summary>
        /// This is the preferred way of using the decorator.
        /// <para>
        /// This method is functionally equivalent with the <see cref="UseDecoratorWithConfigurationAction(IHostBuilder)"/> method.
        /// </para>
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        private static IHostBuilder UseDecoratorWithFactoryMethod(this IHostBuilder hostBuilder)
        {
            return
                hostBuilder.UseServiceProviderFactory(
                    hostBuilderContext =>
                        AutofacServiceProviderFactoryDecorator.CreateDefault(
                            nameof(Company),
                            hostBuilderContext.Configuration,
                            hostBuilderContext.HostingEnvironment
                        )
                );
        }

        /// <remarks>
        /// This is similar to the <see cref="UseDecorator(IHostBuilder)"/> method.
        /// <para>
        /// In this case the configure container action delegate is passed directly to the constructor of the
        /// <see cref="AutofacServiceProviderFactoryDecorator"/> class.
        /// </para>
        /// </remarks>
        private static IHostBuilder UseDecoratorWithConfigurationAction(this IHostBuilder hostBuilder)
        {
            return
                hostBuilder.UseServiceProviderFactory(
                    hostBuilderContext => new AutofacServiceProviderFactoryDecorator(
                        containerBuilder =>
                            containerBuilder
                                .UseDefaultAssemblyRegistry(nameof(Company))
                                .TreatModulesAsServices()
                                .RegisterModuleDependencyInstance(hostBuilderContext.Configuration)
                                .RegisterModuleDependencyInstance(hostBuilderContext.HostingEnvironment)
                                .RegisterModule<DependencyModule>()
                    )
                );
        }

        /// <remarks>
        /// <see cref="AutofacServiceProviderFactoryDecorator"/> will use the original <see cref="IServiceCollection"/> instance
        /// in the background and shares a copy of it with the Autofac modules so that earlier registrations will not be overridden,
        /// hence you do not need to deal with registration override issues due to using a manually created <see cref="IServiceCollection"/>
        /// instance.
        /// <para>
        /// To get the shared <see cref="IServiceCollection"/> instance in your Autofac modules,
        /// <see cref="ModuleContainerBuilderExtensions.TreatModulesAsServices(ContainerBuilder)"/> extension method must be called.
        /// </para>
        /// </remarks>
        private static IHostBuilder UseDecorator(this IHostBuilder hostBuilder)
        {
            return
                hostBuilder
                    .UseServiceProviderFactory(new AutofacServiceProviderFactoryDecorator())
                    .ConfigureContainer<ContainerBuilder>(
                        (hostBuilderContext, containerBuilder) =>
                            containerBuilder
                                .UseDefaultAssemblyRegistry(nameof(Company))
                                .TreatModulesAsServices()
                                .RegisterModuleDependencyInstance(hostBuilderContext.Configuration)
                                .RegisterModuleDependencyInstance(hostBuilderContext.HostingEnvironment)
                                .RegisterModule<DependencyModule>()
                    );
        }

        /// <remarks>
        /// This method shares a manually created <see cref="ServiceCollection"/> instance.
        /// <para>
        /// You had to specify a delegate to <see cref="ModuleOptions.OnModulesRegistered"/> so that
        /// registrations in Autofac modules will not override earlier registrations.
        /// </para>
        /// </remarks>
        private static IHostBuilder UseV3CompatibilityMode(this IHostBuilder hostBuilder)
        {
            return
                hostBuilder
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureContainer<ContainerBuilder>(
                        (hostBuilderContext, containerBuilder) =>
                        {
                            IServiceCollection services = new ServiceCollection();

                            containerBuilder
                                .UseDefaultAssemblyRegistry(nameof(Company))
                                .TreatModulesAsServices()
                                // Avoid overriding ILoggerFactory registration made by UseSerilog() extension method.
                                .ConfigureModuleOptions(
                                    options =>
                                        options.OnModulesRegistered = _ => services.RemoveAll<ILoggerFactory>()
                                )
                                .RegisterModuleDependencyInstance(services)
                                .RegisterModuleDependencyInstance(hostBuilderContext.Configuration)
                                .RegisterModuleDependencyInstance(hostBuilderContext.HostingEnvironment)
                                .RegisterModule<DependencyModule>();
                        }
                    );
        }
    }
}
