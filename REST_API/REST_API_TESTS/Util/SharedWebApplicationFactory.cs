using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * CollectionDefinition and CollectionFixture enable us to share database resource accross multiple test-classes, so the tests can run faster and avoid race-conditions due to xUnit runs tests in parallel by default
     * Reference: Cummings, Neil: "Build a Microservice app with .NET and NextJS from scratch", lecture: 200, Udemy
 */

namespace REST_API_TESTS.Util
{
    [CollectionDefinition("Shared collection")]
    public class SharedWebApplicationFactory : ICollectionFixture<RepositoryTestsWebFactory>
    {
    }
}
