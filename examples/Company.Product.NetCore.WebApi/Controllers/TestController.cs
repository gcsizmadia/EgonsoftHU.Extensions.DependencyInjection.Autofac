// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Linq;

using Company.Product.ComponentA.NetCore;
using Company.Product.ComponentB;
using Company.Product.NetCore.WebApi.Services;

using EgonsoftHU.Extensions.Bcl;
using EgonsoftHU.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Mvc;

namespace Company.Product.NetCore.WebApi.Controllers
{
    /// <summary>
    /// Provides an API endpoint to test the injection of services.
    /// </summary>
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ServiceA serviceA;
        private readonly ServiceB serviceB;
        private readonly ServiceC serviceC;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        /// <param name="serviceA">A singleton instance of the <seealso cref="ServiceA"/> class.</param>
        /// <param name="serviceB">A singleton instance of the <seealso cref="ServiceB"/> class.</param>
        /// <param name="serviceC">A singleton instance of the <seealso cref="ServiceC"/> class.</param>
        public TestController(ServiceA serviceA, ServiceB serviceB, ServiceC serviceC)
        {
            this.serviceA = serviceA;
            this.serviceB = serviceB;
            this.serviceC = serviceC;
        }

        /// <summary>
        /// Gets the welcome messages from all services.
        /// </summary>
        /// <returns>an object with the welcome messages.</returns>
        [HttpGet]
        [Route("api/tests")]
        public IActionResult GetAll()
        {
            DefaultAssemblyRegistry.Current.ThrowIfNull();

            return
                Ok(
                    new
                    {
                        ServiceA = serviceA.GetData(),
                        ServiceB = serviceB.GetMessage(),
                        ServiceC = serviceC.GetMessage(),
                        Assemblies =
                            DefaultAssemblyRegistry
                                .Current
                                .GetAssemblies()
                                .Select(assembly => assembly.GetName().FullName)
                                .ToList()
                    }
                );
        }
    }
}
