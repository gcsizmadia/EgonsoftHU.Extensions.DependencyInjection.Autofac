﻿// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;

namespace Company.Product.Net8.MauiClient.Services
{
    /// <summary>
    /// An Autofac dependency module that registers services of MAUI app.
    /// </summary>
    public class DependencyModule : Module
    {
        /// <summary>
        /// Registers services of MAUI app.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ServiceD>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
