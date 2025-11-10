using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TekusChallenge.Domain.Interfaces;
using TekusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Data;
using TekusChallenge.Infrastructure.Repositories;
using TekusChallenge.Infrastructure.Services;
using TekusChallenge.Infrastructure.Interceptors; 

namespace TekusChallenge.Infrastructure;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("TekusConnection"),
                builder =>
                {
                    builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
        });

        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IProviderRepository, ProviderRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IProviderCustomFieldRepository, ProviderCustomFieldRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IAuthenticationSettings, AuthenticationSettings>();

        services.AddHttpClient<IRestCountriesService, RestCountriesService>();

        return services;
    }
}
