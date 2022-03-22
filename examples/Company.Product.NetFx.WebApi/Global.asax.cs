// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;
using System.Web;
using System.Web.Http;

namespace Company.Product.NetFx.WebApi
{
    /// <summary>
    /// Defines the methods, properties, and events that are common to all application objects in an ASP.NET application.
    /// </summary>
    public class App : HttpApplication
    {
        /// <summary>
        /// The event handler for application start event.
        /// </summary>
        /// <param name="sender">The object that raised this event.</param>
        /// <param name="e">The arguments provided to use with this event.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
