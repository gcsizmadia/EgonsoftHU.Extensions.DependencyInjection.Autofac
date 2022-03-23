// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;
using System.Reflection;

namespace EgonsoftHU.Extensions.DependencyInjection
{
    internal class AssemblyRegistryEntry : IComparable<AssemblyRegistryEntry>, IComparable
    {
        private const int CurrentIsGreaterThanOther = 1;

        internal AssemblyRegistryEntry(Assembly assembly)
        {
            Assembly = assembly;
            FullName = assembly.FullName;
            Name = assembly.GetName().Name;
        }

        internal string Name { get; }

        internal string FullName { get; }

        internal Assembly Assembly { get; }

        public int CompareTo(object obj)
        {
            return obj is AssemblyRegistryEntry assemblyInfo ? CompareTo(assemblyInfo) : CurrentIsGreaterThanOther;
        }

        public int CompareTo(AssemblyRegistryEntry other)
        {
            return other is null ? CurrentIsGreaterThanOther : FullName.CompareTo(other.FullName);
        }
    }
}
