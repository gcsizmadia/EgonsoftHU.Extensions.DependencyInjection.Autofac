// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;

using Android.App;
using Android.Runtime;

using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Company.Product.Net8.MauiClient
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp()
        {
            return MauiProgram.CreateMauiApp();
        }
    }
}
