using TekusChallenge.API.Modules.GlobalException;
using TekusChallenge.API.Services;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.API.Modules.Injection;

public static class InjectionExtension
{
    public static IServiceCollection AddInjection(this IServiceCollection services)
    {
        services.AddTransient<GlobalExceptionHandler>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        return services;
    }
}
