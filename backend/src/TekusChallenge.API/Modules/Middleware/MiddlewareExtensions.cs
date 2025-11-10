using TekusChallenge.API.Modules.GlobalException;

namespace TekusChallenge.API.Modules.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder AddMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandler>();
    }
}
