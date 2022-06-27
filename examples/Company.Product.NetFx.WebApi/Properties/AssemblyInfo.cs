// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Reflection;
using System.Runtime.InteropServices;

#if DEBUG
[assembly: AssemblyConfiguration(".NETFramework,Version=v4.8,Configuration=Debug")]
#else
[assembly: AssemblyConfiguration(".NETFramework,Version=v4.8,Configuration=Release")]
#endif
[assembly: ComVisible(false)]
[assembly: Guid("4b313cc0-1761-4a13-994c-a62a04d61577")]
[assembly: AssemblyCompany("Egonsoft.HU")]
[assembly: AssemblyCopyright("Copyright © 2022 Gabor Csizmadia")]
[assembly: AssemblyProduct("Example for using Egonsoft.HU DependencyInjection Extensions for Autofac")]
[assembly: AssemblyTitle("Company.Product.NetFx.WebApi")]
[assembly: AssemblyDescription("Web API (net48)")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
