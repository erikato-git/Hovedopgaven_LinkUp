using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp_REST_API_TESTS.TestHelpers
{
    public class AuthenticationTestHelper
    {
        public static ClaimsPrincipal GetClaimsPrincipal()
        {
            var claims = new List<Claim> { new Claim("AccountId", GetValidAccountId().ToString() ) };
            var identity = new ClaimsIdentity(claims, "testing");
            return new ClaimsPrincipal(identity);
        }

        public static void ResetHttpContext(ControllerContext controllerContext)
        {
            controllerContext.HttpContext = new DefaultHttpContext();
        }

        public static void SetAccountIdClaimInHttpContext(ControllerContext controllerContext, Guid accountId)
        {
            var claims = new List<Claim>
            {
                new Claim("AccountId", accountId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "testing");
            var principal = new ClaimsPrincipal(identity);

            controllerContext.HttpContext = new DefaultHttpContext
            {
                User = principal
            };
        }

        public static Guid GetValidAccountId()
        {
            return Guid.Parse("617122cf-c317-42c8-9c59-24830c640e6c");
        }

    }
}
