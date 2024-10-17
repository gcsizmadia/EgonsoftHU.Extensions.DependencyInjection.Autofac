// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

namespace Company.Product.NetCore.WebApi.Services
{
    /// <summary>
    /// A service of WebApi
    /// </summary>
    public class ServiceC
    {
        /// <summary>
        /// Gets a welcome message.
        /// </summary>
        /// <returns>a string value that contains the message: <c>Hello from Company.Product.NetCore.WebApi.Services.ServiceC</c></returns>
        public string GetMessage()
        {
            return $"Hello from {typeof(ServiceC).FullName}";
        }
    }
}
