// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Company.Product.ComponentA.NetCore;
using Company.Product.ComponentB;
using Company.Product.Net7.MauiClient.Services;

using EgonsoftHU.Extensions.Bcl;
using EgonsoftHU.Extensions.DependencyInjection;
using EgonsoftHU.Extensions.Logging;

using Microsoft.Maui.Controls;

using ILogger = Serilog.ILogger;

namespace Company.Product.Net7.MauiClient.Views
{
    public partial class DependencyInjectionTestPage : ContentPage
    {
        private readonly ServiceA serviceA;
        private readonly ServiceB serviceB;
        private readonly ServiceC serviceC;
        private readonly ILogger logger;

        public DependencyInjectionTestPage(ServiceA serviceA, ServiceB serviceB, ServiceC serviceC, ILogger logger)
        {
            this.serviceA = serviceA;
            this.serviceB = serviceB;
            this.serviceC = serviceC;
            this.logger = logger;

            InitializeComponent();
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
                    Assemblies =
                        DefaultAssemblyRegistry
                            .Current
                            .GetAssemblies()
                            .Select(assembly => assembly.GetName().FullName)
                            .ToList()
                };

            logger.Here().Verbose("Info=[{@Info}]", info);

            string appInfo =
                JsonSerializer.Serialize(
                    info,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)
                    {
                        WriteIndented = true
                    }
                );

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
