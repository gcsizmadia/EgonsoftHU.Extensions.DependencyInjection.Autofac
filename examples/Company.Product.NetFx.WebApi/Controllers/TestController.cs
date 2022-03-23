// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Linq;
using System.Web.Http;

using Company.Product.ComponentA;
using Company.Product.ComponentB;
using Company.Product.NetFx.WebApi.Services;

using EgonsoftHU.Extensions.DependencyInjection;

namespace Company.Product.NetFx.WebApi.Controllers
{
    /// <summary>
    /// Provides an API endpoint to test the injection of services.
    /// </summary>
    public class TestController : ApiController
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
        public IHttpActionResult GetAll()
        {
            return
                Ok(
                    new
                    {
                        ServiceA = serviceA.GetMessage(),
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
