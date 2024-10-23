// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

namespace Company.Product.Net8.WebApi.Services
{
    /// <summary>
    /// A service of WebApi
    /// </summary>
    public class ServiceD
    {
        /// <summary>
        /// Gets a welcome message.
        /// </summary>
        /// <returns>a string value that contains a message including the full name of this service type.</returns>
        public string GetMessage()
        {
            return $"Hello from {typeof(ServiceD).FullName}";
        }
    }
}
