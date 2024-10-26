﻿// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.IO;
using System.Reflection;

using EgonsoftHU.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
#if WINDOWS
using Microsoft.Maui.LifecycleEvents;

using WinUIEx;
#endif

using Serilog;

namespace Company.Product.Net8.MauiClient
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
                AutofacServiceProviderFactoryDecorator.CreateDefault(
                    nameof(Company),
                    (IConfiguration)builder.Configuration
                )
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
                Assembly.GetExecutingAssembly().GetManifestResourceStream($"Company.Product.Net8.MauiClient.appsettings.json")
                ??
                new MemoryStream();

            return new ConfigurationBuilder().AddJsonStream(streamAppSettingsJson).Build();
        }
    }
}
