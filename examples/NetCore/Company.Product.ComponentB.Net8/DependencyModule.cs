// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;

namespace Company.Product.ComponentB.Net8
{
    /// <summary>
    /// An Autofac dependency module that registers services of ComponentB.
    /// </summary>
    public class DependencyModule : Module
    {
        /// <summary>
        /// Registers services of ComponentB.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ServiceB>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
