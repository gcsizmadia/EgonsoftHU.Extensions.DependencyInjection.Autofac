// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;
using System.Collections;
using System.Collections.Generic;

using Autofac;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    internal class ConfigureContainerActionsBuilder : IEnumerable
    {
        private readonly List<Action<ContainerBuilder>> actions =
#if LANGVERSION12_0_OR_GREATER
            []
#else
            new()
#endif
            ;

        internal void Add<TDependency>(TDependency dependency)
            where TDependency : class
        {
            actions.Add(containerBuilder => containerBuilder.RegisterModuleDependencyInstance(dependency));
        }

        internal IReadOnlyCollection<Action<ContainerBuilder>> Build()
        {
            return actions.AsReadOnly();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)actions).GetEnumerator();
        }
    }
}
