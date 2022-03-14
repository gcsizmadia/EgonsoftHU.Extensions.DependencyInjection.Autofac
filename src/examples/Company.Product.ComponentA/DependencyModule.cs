// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;

namespace Company.Product.ComponentA
{
    /// <summary>
    /// An Autofac dependency module that registers services of ComponentA.
    /// </summary>
    public class DependencyModule : Module
    {
        /// <summary>
        /// Registers services of ComponentA.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ServiceA>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
