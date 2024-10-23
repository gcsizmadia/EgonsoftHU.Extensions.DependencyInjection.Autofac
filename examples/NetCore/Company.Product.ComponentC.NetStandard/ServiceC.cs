// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

namespace Company.Product.ComponentC.NetStandard
{
    /// <summary>
    /// A service of ComponentB
    /// </summary>
    public class ServiceC
    {
        /// <summary>
        /// Gets a welcome message.
        /// </summary>
        /// <returns>a string value that contains a message including the full name of this service type.</returns>
        public string GetMessage()
        {
            return $"Hello from {typeof(ServiceC).FullName}";
        }
    }
}
