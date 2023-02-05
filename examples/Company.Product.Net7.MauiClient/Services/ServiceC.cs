// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

namespace Company.Product.Net7.MauiClient.Services
{
    /// <summary>
    /// A service of MAUI app
    /// </summary>
    public class ServiceC
    {
        private readonly string? typeFullName = typeof(ServiceC).FullName;

        /// <summary>
        /// Gets a welcome message.
        /// </summary>
        /// <returns>a string value that contains the message: <c>Hello from Company.Product.Net7.MauiClient.Services.ServiceC</c></returns>
        public string GetMessage()
        {
            return $"Hello from {typeFullName}";
        }
    }
}
