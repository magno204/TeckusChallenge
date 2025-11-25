using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TekusChallenge.Infrastructure.Data;
using TekusChallenge.Infrastructure.Interceptors;

namespace TekusChallenge.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"TekusTestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["KeyVaultName"] = "",
                ["ConnectionStrings:TekusConnection"] = "InMemoryDatabase",
                ["Config:OriginCors"] = "http://localhost:4200",
                ["Config:Secret"] = "TestSecretKey+_)(*&^%$#@!)(JJGGG$$##+___*skldjgfndifgbiebrgklejrwglrgblkj",
                ["Config:Issuer"] = "Tekus.co",
                ["Config:Audience"] = "Tekus-Client",
                ["Config:ExpirationMinutes"] = "60",
                ["RateLimiting:PermitLimit"] = "1000",
                ["RateLimiting:Window"] = "60",
                ["RateLimiting:QueueLimit"] = "100",
                ["TestCredentials:Username"] = "admin",
                ["TestCredentials:Password"] = "admin",
                ["ExternalApis:RestCountriesBaseUrl"] = "https://restcountries.com/v3.1/",
                ["ExternalApis:RestCountriesAllCountriesEndpoint"] = "all?fields=name,flags,cca2,cca3",
                ["ExternalApis:RestCountriesCountryByCodeEndpoint"] = "alpha/{code}?fields=name,flags,cca2,cca3"
            });
        });

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.EnsureCreated();
        SeedTestData(db);

        return host;
    }

    private static void SeedTestData(ApplicationDbContext context)
    {
        if (!context.Countries.Any())
        {
            context.Countries.AddRange(
                new TekusChallenge.Domain.Entities.Country
                {
                    Code = "CO",
                    CodeAlpha3 = "COL",
                    Name = "Colombia",
                    Flag = "https://flagcdn.com/co.svg"
                },
                new TekusChallenge.Domain.Entities.Country
                {
                    Code = "US",
                    CodeAlpha3 = "USA",
                    Name = "United States",
                    Flag = "https://flagcdn.com/us.svg"
                },
                new TekusChallenge.Domain.Entities.Country
                {
                    Code = "MX",
                    CodeAlpha3 = "MEX",
                    Name = "Mexico",
                    Flag = "https://flagcdn.com/mx.svg"
                }
            );
        }

        context.SaveChanges();
    }
}
