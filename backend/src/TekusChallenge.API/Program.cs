using TekusChallenge.Application;
using TekusChallenge.Infrastructure;
using TekusChallenge.API.Modules.Injection;
using TekusChallenge.API.Modules.Feature;
using TekusChallenge.API.Modules.Versioning;
using TekusChallenge.API.Modules.Swagger;
using TekusChallenge.API.Modules.RateLimiter;
using Asp.Versioning.ApiExplorer;
using TekusChallenge.API.Modules.Middleware;
using TekusChallenge.API.Modules.Authentication;
using TekusChallenge.API.Modules.Database;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Azure Key Vault
var keyVaultName = builder.Configuration["KeyVaultName"];
var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential(),
    new AzureKeyVaultConfigurationOptions
    {
        ReloadInterval = TimeSpan.FromMinutes(1)
    });

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddInjection();
builder.Services.AddFeature(builder.Configuration);
builder.Services.AddVersioning();
builder.Services.AddSwagger();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddRateLimiting(builder.Configuration);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.ApplyMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            $"Tekus API {description.GroupName.ToUpperInvariant()}");
        }
    });
    app.UseReDoc(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.DocumentTitle = $"Tekus API {description.GroupName.ToUpperInvariant()} Documentation";
            options.SpecUrl = $"/swagger/{description.GroupName}/swagger.json";
        }
    });
    
}

app.UseHttpsRedirection();
app.UseCors("EnableCORS");
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
//app.UseRequestTimeouts();
app.MapControllers();
app.AddMiddleware();
app.Run();
