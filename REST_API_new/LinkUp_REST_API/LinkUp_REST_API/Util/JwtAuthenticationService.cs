using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LinkUp_REST_API.Util
{
    /*
     * TODO: Find a better place for this class to enforce better cohesion
     */
    public class JwtAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtAuthenticationService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public void ConfigureJwtAuthentication(AuthenticationBuilder builder)
        {
            builder.AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    ValidIssuer = _jwtSettings.Issuers,
                    ValidAudience = _jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero,
                };
            });
        }
    }

}
