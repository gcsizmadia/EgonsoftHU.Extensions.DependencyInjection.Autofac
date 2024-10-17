// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

namespace Company.Product.ComponentB
{
    /// <summary>
    /// A service of ComponentB
    /// </summary>
    public class ServiceB
    {
        /// <summary>
        /// Gets a welcome message.
        /// </summary>
        /// <returns>a string value that contains the message: <c>Hello from Company.Product.ComponentB.ServiceB</c></returns>
        public string GetMessage()
        {
            return $"Hello from {typeof(ServiceB).FullName}";
        }
    }
}
