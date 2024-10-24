// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Company.Product.ComponentA.Net6;
using Company.Product.ComponentB.Net8;
using Company.Product.ComponentC.NetStandard;
using Company.Product.Net8.MauiClient.Services;

using EgonsoftHU.Extensions.Bcl;
using EgonsoftHU.Extensions.DependencyInjection;
using EgonsoftHU.Extensions.Logging;

using Microsoft.Maui.Controls;
#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
#endif

using ILogger = Serilog.ILogger;

namespace Company.Product.Net8.MauiClient.Views
{
    public partial class DependencyInjectionTestPage : ContentPage
    {
        private readonly ServiceA serviceA;
        private readonly ServiceB serviceB;
        private readonly ServiceC serviceC;
        private readonly ServiceD serviceD;
        private readonly ILogger logger;

        private readonly JsonSerializerOptions options =
            new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true
            };

        public DependencyInjectionTestPage(
            ServiceA serviceA,
            ServiceB serviceB,
            ServiceC serviceC,
            ServiceD serviceD,
            ILogger logger
        )
        {
            this.serviceA = serviceA;
            this.serviceB = serviceB;
            this.serviceC = serviceC;
            this.serviceD = serviceD;
            this.logger = logger;

            InitializeComponent();

#if WINDOWS
            AppInfo.HandlerChanged +=
                (sender, e) =>
                {
                    if (sender is not WebView webView)
                    {
                        return;
                    }

                    if (webView.Handler?.PlatformView is not WebView2 webView2)
                    {
                        return;
                    }

                    webView2.CoreWebView2Initialized +=
                        (sender, e) =>
                        {
                            CoreWebView2Settings settings = webView2.CoreWebView2.Settings;

                            settings.AreDefaultScriptDialogsEnabled = false;
                            settings.AreDevToolsEnabled = false;
                            settings.AreHostObjectsAllowed = false;
                            settings.IsScriptEnabled = false;
                            settings.IsWebMessageEnabled = false;
                        };
                };
#endif
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            DefaultAssemblyRegistry.Current.ThrowIfNull();

            AppInfo.Source = GetWebViewSource("Getting info...");

            await Task.Delay(1000);

            var info =
                new
                {
                    ServiceA = serviceA.GetData(),
                    ServiceB = serviceB.GetMessage(),
                    ServiceC = serviceC.GetMessage(),
                    ServiceD = serviceD.GetMessage(),
                    Assemblies =
                        DefaultAssemblyRegistry
                            .Current
                            .GetAssemblies()
                            .Select(assembly => assembly.GetName().FullName)
                            .ToList()
                };

            logger.Here().Verbose("Info=[{@Info}]", info);

            string appInfo = JsonSerializer.Serialize(info, options);

            AppInfo.Source = GetWebViewSource(appInfo);
        }

        private static HtmlWebViewSource GetWebViewSource(string content)
        {
            return new HtmlWebViewSource()
            {
                Html = $"<!DOCTYPE html><html><head><title>Dependency Injection Test</title></head><body><pre>{content}</pre></body></html>"
            };
        }
    }
}
