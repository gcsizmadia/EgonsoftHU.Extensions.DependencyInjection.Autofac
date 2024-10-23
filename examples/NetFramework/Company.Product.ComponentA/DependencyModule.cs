// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

#nullable enable

using System.Collections.Specialized;

using Autofac;

namespace Company.Product.ComponentA
{
    /// <summary>
    /// An Autofac dependency module that registers services of ComponentA.
    /// </summary>
    public class DependencyModule : Module
    {
        public NameValueCollection? AppSettings { get; set; }

        /// <summary>
        /// Registers services of ComponentA.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ServiceA>()
                .AsSelf()
                .SingleInstance()
                .WithParameter(new NamedParameter("environmentName", AppSettings?["EnvironmentName"] ?? "N/A"))
                .WithParameter(new NamedParameter("welcomeMessage", AppSettings?["ServiceA:WelcomeMessage"] ?? "N/A"));
        }
    }
}
