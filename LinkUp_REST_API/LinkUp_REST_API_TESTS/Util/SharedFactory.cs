using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LinkUp_REST_API_TESTS.Util
{
    [CollectionDefinition("Shared collection")]
    public class SharedFactory : ICollectionFixture<ApplicationFactory>
    {
    }
}
