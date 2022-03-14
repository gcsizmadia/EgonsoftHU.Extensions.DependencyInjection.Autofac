// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

namespace Company.Product.ComponentA
{
    /// <summary>
    /// A service of ComponentA
    /// </summary>
    public class ServiceA
    {
        /// <summary>
        /// Gets a welcome message.
        /// </summary>
        /// <returns>a string value that contains the message: <c>Hello from Company.Product.ComponentA.ServiceA</c></returns>
        public string GetMessage()
        {
            return $"Hello from {typeof(ServiceA).FullName}";
        }
    }
}
