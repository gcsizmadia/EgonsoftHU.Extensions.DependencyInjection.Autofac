// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Collections.Generic;
using System.Reflection;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    /// <summary>
    /// Defines a mechanism for retrieving all assemblies loaded into the current application domain.
    /// </summary>
    public interface IAssemblyRegistry
    {
        /// <summary>
        /// Gets all assemblies loaded into the current application domain.
        /// </summary>
        /// <returns>an array of assemblies that are loaded into the current application domain.</returns>
        IEnumerable<Assembly> GetAssemblies();
    }
}
