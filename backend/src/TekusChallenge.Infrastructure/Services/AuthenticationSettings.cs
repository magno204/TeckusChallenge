using Microsoft.Extensions.Configuration;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Infrastructure.Services;

public class AuthenticationSettings : IAuthenticationSettings
{
    private readonly IConfiguration _configuration;

    public AuthenticationSettings(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetValidUsername()
    {
        return _configuration["TestCredentials:Username"] ?? "admin";
    }

    public string GetValidPassword()
    {
        return _configuration["TestCredentials:Password"] ?? "admin";
    }
}