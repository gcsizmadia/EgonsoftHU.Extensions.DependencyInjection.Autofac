// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Web.Http;

using Serilog;

namespace Company.Product.NetFx.WebApi
{
    partial class WebApiConfig
    {
        public static void ConfigureSerilog(this HttpConfiguration _)
        {
            const string OutputTemplate =
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fffffff zzz} [{Level:u3}] [{SourceContext}]::[{SourceMember}] {Message:lj}{NewLine}{Exception}";

            Log.Logger =
                new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Console(outputTemplate: OutputTemplate)
                    .WriteTo.Debug(outputTemplate: OutputTemplate)
                    .CreateLogger();
        }
    }
}
