using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// Neil, Microservice: lecture 188

namespace REST_API_TESTS.TestHelpers
{
    public class ClaimsTestHelper
    {
        public static ClaimsPrincipal GetClaimsPrincipal()
        {
            var guid = Guid.NewGuid();
            var claims = new List<Claim>
            {
                new Claim("AccountId", guid.ToString())     // has to be string
            };

            var identity = new ClaimsIdentity(claims, "testing");

            return new ClaimsPrincipal(identity);
        }
    }
}
