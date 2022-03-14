// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Egonsoft.HU")]
[assembly: AssemblyProduct("Egonsoft.HU DependencyInjection Extensions for Autofac")]
[assembly: AssemblyCopyright("Copyright © 2022 Gabor Csizmadia")]

#if DEBUG
#if NETSTANDARD2_0
[assembly: AssemblyConfiguration(".NETStandard,Version=v2.0,Configuration=Debug")]
#elif NET48
[assembly: AssemblyConfiguration(".NETFramework,Version=v4.8,Configuration=Debug")]
#elif NET6_0
[assembly: AssemblyConfiguration(".NETCoreApp,Version=v6.0,Configuration=Debug")]
#else
#error Missing AssemblyConfigurationAttribute for this target.
#endif
#else
#if NETSTANDARD2_0
[assembly: AssemblyConfiguration(".NETStandard,Version=v2.0,Configuration=Release")]
#elif NET48
[assembly: AssemblyConfiguration(".NETFramework,Version=v4.8,Configuration=Release")]
#elif NET6_0
[assembly: AssemblyConfiguration(".NETCoreApp,Version=v6.0,Configuration=Release")]
#else
#error Missing AssemblyConfigurationAttribute for this target.
#endif
#endif

[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
