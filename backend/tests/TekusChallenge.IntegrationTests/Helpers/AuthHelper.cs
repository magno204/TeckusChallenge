using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Tekus.Transversal;
using TekusChallenge.Application.UseCases.Auth.Commands.Login;

namespace TekusChallenge.IntegrationTests.Helpers;

/// <summary>
/// Helper for handling authentication in integration tests
/// </summary>
public static class AuthHelper
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Gets a valid JWT token to use in tests
    /// </summary>
    public static async Task<string> GetAuthTokenAsync(HttpClient client, string username = "admin", string password = "admin")
    {
        var loginCommand = new LoginCommand
        {
            Username = username,
            Password = password
        };

        var response = await client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Response<LoginResult>>(content, JsonOptions);

        return result?.Data?.Token ?? throw new InvalidOperationException("Could not obtain authentication token");
    }

    /// <summary>
    /// Configures the HTTP client with the authentication token
    /// </summary>
    public static async Task<HttpClient> GetAuthenticatedClientAsync(HttpClient client)
    {
        var token = await GetAuthTokenAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    /// <summary>
    /// Sets the authorization header on an HttpClient
    /// </summary>
    public static void SetAuthorizationHeader(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
