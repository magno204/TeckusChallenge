using Microsoft.AspNetCore.Http.Timeouts;
using System.Text.Json.Serialization;

namespace TekusChallenge.API.Modules.Feature;

public static class FeatureExtension
{
    public static IServiceCollection AddFeature(this IServiceCollection services, IConfiguration configuration)
    {
        string myPolicy = "EnableCORS";

        services.AddCors(o => o.AddPolicy(myPolicy, builder =>
        {
            builder.WithOrigins(configuration["Config:OriginCors"]!)
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        }));

        services.AddControllers().AddJsonOptions(opts =>
        {
            var enumConverter = new JsonStringEnumConverter();
            opts.JsonSerializerOptions.Converters.Add(enumConverter);
            opts.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        });

        //services.AddRequestTimeouts(options =>
        //{
        //    options.DefaultPolicy = new RequestTimeoutPolicy { Timeout = TimeSpan.FromMilliseconds(1500) };
        //    options.AddPolicy("CustomPolicy", TimeSpan.FromMilliseconds(2000));
        //});

        return services;
    }
}
