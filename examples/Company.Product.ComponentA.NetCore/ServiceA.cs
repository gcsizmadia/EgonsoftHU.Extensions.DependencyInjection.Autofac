// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Microsoft.Extensions.Options;

namespace Company.Product.ComponentA.NetCore
{
    /// <summary>
    /// A service of ComponentA
    /// </summary>
    public class ServiceA
    {
        private readonly string environmentName;
        private readonly ServiceAOptions options;

        public ServiceA(string environmentName, IOptions<ServiceAOptions> options)
        {
            this.environmentName = environmentName;
            this.options = options.Value;
        }

        /// <summary>
        /// Gets a welcome message and some extra info.
        /// </summary>
        /// <returns>
        /// an object array:<br/>
        /// - a string value that contains the message: <c>Hello from Company.Product.ComponentA.ServiceA</c><br/>
        /// - an object with <c>EnvironmentName</c> property<br/>
        /// - the current instance of the <see cref="ServiceAOptions"/> type
        /// </returns>
        public object[] GetData()
        {
            return new object[]
            {
                $"Hello from {typeof(ServiceA).FullName}",
                new
                {
                    EnvironmentName = environmentName,
                    ServiceAOptions = options
                },
            };
        }
    }
}
