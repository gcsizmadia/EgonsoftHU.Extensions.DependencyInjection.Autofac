// Copyright � 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;

using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Company.Product.Net7.MauiClient
{
    internal class Program : MauiApplication
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}