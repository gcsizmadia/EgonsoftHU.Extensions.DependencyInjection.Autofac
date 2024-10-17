// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Linq;

using Autofac;

using Microsoft.Maui.Controls;

namespace Company.Product.Net7.MauiClient.Views
{
    /// <summary>
    /// An Autofac dependency module that registers views of MAUI app.
    /// </summary>
    public class DependencyModule : Module
    {
        /// <summary>
        /// Registers views of MAUI app.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            ThisAssembly
                .GetExportedTypes()
                .Where(type => typeof(ContentPage).IsAssignableFrom(type) && !type.IsAbstract)
                .ToList()
                .ForEach(type => builder.RegisterType(type).AsSelf().SingleInstance());
        }
    }
}
