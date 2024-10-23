// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Diagnostics;
using System.Linq;

using Company.Product.ComponentA.Net6;
using Company.Product.ComponentB.Net8;
using Company.Product.ComponentC.NetStandard;
using Company.Product.Net8.WebApi.Services;

using EgonsoftHU.Extensions.Bcl;
using EgonsoftHU.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Company.Product.Net8.WebApi.Controllers
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
        private readonly ServiceD serviceD;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        /// <param name="serviceA">A singleton instance of the <seealso cref="ServiceA"/> class.</param>
        /// <param name="serviceB">A singleton instance of the <seealso cref="ServiceB"/> class.</param>
        /// <param name="serviceC">A singleton instance of the <seealso cref="ServiceC"/> class.</param>
        /// <param name="serviceD">A singleton instance of the <seealso cref="ServiceD"/> class.</param>
        public TestController(ServiceA serviceA, ServiceB serviceB, ServiceC serviceC, ServiceD serviceD)
        {
            this.serviceA = serviceA;
            this.serviceB = serviceB;
            this.serviceC = serviceC;
            this.serviceD = serviceD;
        }

        /// <summary>
        /// Gets the welcome messages from all services.
        /// </summary>
        /// <returns>an object with the welcome messages.</returns>
        [HttpGet]
        [Route("api/tests")]
        public IActionResult GetAll([FromServices] ILoggerFactory loggerFactory)
        {
            DefaultAssemblyRegistry.Current.ThrowIfNull();

            Debug.WriteLine(TypeHelper.GetTypeName(loggerFactory.GetType()));

            return
                Ok(
                    new
                    {
                        ServiceA = serviceA.GetData(),
                        ServiceB = serviceB.GetMessage(),
                        ServiceC = serviceC.GetMessage(),
                        ServiceD = serviceD.GetMessage(),
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
