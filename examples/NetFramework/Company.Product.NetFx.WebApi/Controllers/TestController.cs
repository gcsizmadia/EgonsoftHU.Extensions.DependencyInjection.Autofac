// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Linq;
using System.Web.Http;

using Company.Product.ComponentA;
using Company.Product.NetFx.WebApi.Services;

using EgonsoftHU.Extensions.Bcl;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        /// <param name="serviceA">A singleton instance of the <seealso cref="ServiceA"/> class.</param>
        /// <param name="serviceB">A singleton instance of the <seealso cref="ServiceB"/> class.</param>
        public TestController(ServiceA serviceA, ServiceB serviceB)
        {
            this.serviceA = serviceA;
            this.serviceB = serviceB;
        }

        /// <summary>
        /// Gets the welcome messages from all services.
        /// </summary>
        /// <returns>an object with the welcome messages.</returns>
        [HttpGet]
        [Route("api/tests")]
        public IHttpActionResult GetAll()
        {
            DefaultAssemblyRegistry.Current.ThrowIfNull();

            return
                Ok(
                    new
                    {
                        ServiceA = serviceA.GetData(),
                        ServiceB = serviceB.GetMessage(),
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
