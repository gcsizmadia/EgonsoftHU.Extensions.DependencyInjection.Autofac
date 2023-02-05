// Copyright � 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using ObjCRuntime;

using UIKit;

namespace Company.Product.Net7.MauiClient
{
    public class Program
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}