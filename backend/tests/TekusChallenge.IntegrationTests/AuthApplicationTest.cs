using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TekusChallenge.Application.UseCases.Auth.Commands.Login;
using Tekus.Transversal;

namespace TekusChallenge.IntegrationTests;

public class AuthApplicationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public AuthApplicationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Login_WhenFieldsAreEmpty_ShouldReturnBadRequest()
    {
        var loginCommand = new LoginCommand
        {
            Username = string.Empty,
            Password = string.Empty
        };

        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WhenFieldsAreTooShort_ShouldReturnBadRequest()
    {
        var loginCommand = new LoginCommand
        {
            Username = "ab", 
            Password = "12"  
        };

        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WhenCredentialsAreIncorrect_ShouldReturnUnauthorized()
    {
        var loginCommand = new LoginCommand
        {
            Username = "usuarioIncorrecto",
            Password = "passwordIncorrecto"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Invalid username or password", content);
    }

    [Fact]
    public async Task Login_WhenCredentialsAreCorrect_ShouldReturnOkWithToken()
    {
        var loginCommand = new LoginCommand
        {
            Username = "admin",
            Password = "admin"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Response<LoginResult>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Authentication successful", result.Message);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Token);
        Assert.NotEmpty(result.Data.Token);
        Assert.Equal("admin", result.Data.Username);
        Assert.True(result.Data.ExpiresAt > DateTime.UtcNow);
    }

    [Fact]
    public async Task Login_WhenUsernameIsCorrectButPasswordIsIncorrect_ShouldReturnUnauthorized()
    {
        var loginCommand = new LoginCommand
        {
            Username = "admin",
            Password = "passwordIncorrecto123"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WhenPasswordIsCorrectButUsernameIsIncorrect_ShouldReturnUnauthorized()
    {
        var loginCommand = new LoginCommand
        {
            Username = "usuarioIncorrecto",
            Password = "admin"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
