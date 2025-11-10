using Microsoft.AspNetCore.RateLimiting;

namespace TekusChallenge.API.Modules.RateLimiter;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var fixedWindowPolicy = "fixedWindow";
        services.AddRateLimiter(configureOptions =>
        {
            configureOptions.AddFixedWindowLimiter(policyName: fixedWindowPolicy, fixedWindow =>
            {
                fixedWindow.PermitLimit = int.Parse(configuration["RateLimiting:PermitLimit"]!);
                fixedWindow.Window = TimeSpan.FromSeconds(int.Parse(configuration["RateLimiting:Window"]!));
                fixedWindow.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                fixedWindow.QueueLimit = int.Parse(configuration["RateLimiting:QueueLimit"]!);
            });
            configureOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            configureOptions.OnRejected = async (context, cancellationToken) =>
            {
                var origin = context.HttpContext.Request.Headers["Origin"].ToString();
                if (!string.IsNullOrEmpty(origin))
                {
                    context.HttpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    context.HttpContext.Response.Headers.Append("Access-Control-Allow-Methods", "*");
                    context.HttpContext.Response.Headers.Append("Access-Control-Allow-Headers", "*");
                }
                
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken);
            };
        });

        return services;
    }
}
