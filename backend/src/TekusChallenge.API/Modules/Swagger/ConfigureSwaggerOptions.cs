using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TekusChallenge.API.Modules.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        this.provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "Tekus API",
            Version = description.ApiVersion.ToString(),
            Description = "API Tekus",
            Contact = new OpenApiContact()
            {
                Email = "contact@tekus.co",
                Name = "Tekus Team",
                Url = new Uri("https://github.com/magno204/TekusChallenge")
            },
            TermsOfService = new Uri("https://github.com/magno204/TekusChallenge/README.md"),
            License = new OpenApiLicense()
            {
                Name = "MIT",
                //Url = new Uri("https://example.com/license")
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version is deprecated.";
        }

        return info;
    }
}
