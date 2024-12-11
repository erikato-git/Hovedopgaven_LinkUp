using System.Runtime.CompilerServices;
using System.Threading.RateLimiting;

namespace LinkUp_REST_API.Extensions
{
    public static class RateLimiterExtension
    {
        /*
         * source: https://www.youtube.com/watch?v=PIfGHbvuAtM&list=TLPQMTAxMjIwMjSepc9wZ80Stw
         */
        public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // Rate limiting based on public IP, applied to all endpoints
                options.AddPolicy("public-ip", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromSeconds(5),
                        }
                    )
                );

            });

            return services;
        }
    }
}
