// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

namespace Company.Product.ComponentA.NetFx
{
    /// <summary>
    /// A service of ComponentA
    /// </summary>
    public class ServiceA
    {
        private readonly string environmentName;
        private readonly string welcomeMessage;

        public ServiceA(string environmentName, string welcomeMessage)
        {
            this.environmentName = environmentName;
            this.welcomeMessage = welcomeMessage;
        }

        public object[] GetData()
        {
            return new object[]
            {
                $"Hello from {typeof(ServiceA).FullName}",
                new
                {
                    EnvironmentName = environmentName,
                    WelcomeMessage = welcomeMessage
                }
            };
        }
    }
}
