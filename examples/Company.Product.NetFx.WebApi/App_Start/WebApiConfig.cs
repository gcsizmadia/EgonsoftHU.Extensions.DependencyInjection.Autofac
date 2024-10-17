// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Web.Http;

namespace Company.Product.NetFx.WebApi
{
    /// <summary>
    /// Provides methods to configure the ASP.NET Web API application.
    /// </summary>
    public static partial class WebApiConfig
    {
        /// <summary>
        /// Configures the ASP.NET Web API application.
        /// </summary>
        /// <param name="httpConfiguration"></param>
        public static void Register(HttpConfiguration httpConfiguration)
        {
            // Web API configuration and services
            httpConfiguration.ConfigureSerilog();
            httpConfiguration.ConfigureAutofac();

            // Web API routes
            httpConfiguration.MapHttpAttributeRoutes();

            httpConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
