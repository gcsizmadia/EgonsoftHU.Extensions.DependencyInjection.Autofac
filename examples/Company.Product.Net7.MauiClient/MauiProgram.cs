// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.IO;
using System.Reflection;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using EgonsoftHU.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
#if WINDOWS
using Microsoft.Maui.LifecycleEvents;

using WinUIEx;
#endif

using Serilog;

namespace Company.Product.Net7.MauiClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            IConfiguration embeddedAppSettingsJson = LoadAppSettingsJsonEmbeddedResource();

            Log.Logger =
                new LoggerConfiguration()
                    .ReadFrom.Configuration(embeddedAppSettingsJson)
                    .CreateLogger();

            MauiAppBuilder builder = MauiApp.CreateBuilder();

            builder.Configuration.AddConfiguration(embeddedAppSettingsJson);

            builder.Logging.AddSerilog(dispose: true);

            builder.ConfigureContainer(
                new AutofacServiceProviderFactory(),
                containerBuilder =>
                {
                    IServiceCollection services = new ServiceCollection();

                    containerBuilder
                        .UseDefaultAssemblyRegistry(nameof(Company), nameof(EgonsoftHU))
                        .TreatModulesAsServices()
                        .RegisterModuleDependencyInstance(services)
                        .RegisterModuleDependencyInstance<IConfiguration>(builder.Configuration)
                        .RegisterModule<DependencyModule>();
                }
            );

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(
                    fonts =>
                    fonts
                        .AddFont("OpenSans-Regular.ttf", "OpenSansRegular")
                        .AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold")
                        .AddFont("consola.ttf", "Consolas")
                );

#if WINDOWS
            builder.ConfigureLifecycleEvents(
                lifecycleBuilder =>
                {
                    lifecycleBuilder.AddWindows(
                        windowsLifecycleBuilder =>
                        {
                            windowsLifecycleBuilder.OnWindowCreated(
                                window =>
                                {
                                    const double PreferredWidth = 1024;
                                    const double PreferredHeight = 768;

                                    window.CenterOnScreen(PreferredWidth, PreferredHeight);
                                }
                            );
                        }
                    );
                }
            );
#endif

            return builder.Build();
        }

        private static IConfiguration LoadAppSettingsJsonEmbeddedResource()
        {
            using Stream streamAppSettingsJson =
                Assembly.GetExecutingAssembly().GetManifestResourceStream($"Company.Product.Net7.MauiClient.appsettings.json")
                ??
                new MemoryStream();

            return new ConfigurationBuilder().AddJsonStream(streamAppSettingsJson).Build();
        }
    }
}
