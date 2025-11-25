using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Application.UseCases.Providers.Commands.CreateProvider;
using TekusChallenge.Application.UseCases.Providers.Commands.UpdateProvider;
using TekusChallenge.IntegrationTests.Helpers;

namespace TekusChallenge.IntegrationTests;

public class ProvidersIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ProvidersIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    #region Creation Tests (POST)

    [Fact]
    public async Task CreateProvider_WhenDataIsValid_ShouldReturnCreatedWithProvider()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor de Prueba",
            Email = "proveedor@test.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/providers", command);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Response<ProviderDto>>(content, _jsonOptions);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("123456789", result.Data.Nit);
        Assert.Equal("Proveedor de Prueba", result.Data.Name);
        Assert.Equal("proveedor@test.com", result.Data.Email);
        Assert.NotEqual(Guid.Empty, result.Data.Id);
    }

    [Fact]
    public async Task CreateProvider_WhenDataIsValidWithCustomFields_ShouldCreateProviderWithFields()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        var command = new CreateProviderCommand
        {
            Nit = "987654321",
            Name = "Proveedor con Campos",
            Email = "campos@test.com",
            CustomFields = new List<CreateProviderCustomFieldDto>
            {
                new()
                {
                    FieldName = "Telefono",
                    FieldValue = "3001234567",
                    FieldType = "text",
                    Description = "Teléfono de contacto",
                    DisplayOrder = 1
                },
                new()
                {
                    FieldName = "Website",
                    FieldValue = "https://example.com",
                    FieldType = "url",
                    DisplayOrder = 2
                }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/providers", command);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Response<ProviderDto>>(content, _jsonOptions);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.CustomFields);
        Assert.Equal(2, result.Data.CustomFields.Count);
    }

    [Fact]
    public async Task CreateProvider_WhenNitIsEmpty_ShouldReturnBadRequest()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        var command = new CreateProviderCommand
        {
            Nit = "",
            Name = "Proveedor",
            Email = "test@test.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/providers", command);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateProvider_WhenNameIsTooShort_ShouldReturnBadRequest()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "A",
            Email = "test@test.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/providers", command);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateProvider_WhenEmailIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        var command = new CreateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor Válido",
            Email = "email-invalido"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/providers", command);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateProvider_WhenNitContainsLetters_ShouldReturnBadRequest()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        var command = new CreateProviderCommand
        {
            Nit = "ABC123456",
            Name = "Proveedor",
            Email = "test@test.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/providers", command);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Get Tests (GET)

    [Fact]
    public async Task GetAllProviders_WhenProvidersExist_ShouldReturnPaginatedList()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        
        var createCommand = new CreateProviderCommand
        {
            Nit = "111222333",
            Name = "Proveedor Para Listar",
            Email = "listar@test.com"
        };
        await _client.PostAsJsonAsync("/api/v1/providers", createCommand);

        // Act
        var response = await _client.GetAsync("/api/v1/providers?pageNumber=1&pageSize=10");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponsePagination<IEnumerable<ProviderDto>>>(content, _jsonOptions);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Any());
    }

    [Fact]
    public async Task GetProviderById_WhenProviderExists_ShouldReturnProvider()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        
        var createCommand = new CreateProviderCommand
        {
            Nit = "444555666",
            Name = "Proveedor Por ID",
            Email = "porid@test.com"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/v1/providers", createCommand);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdProvider = JsonSerializer.Deserialize<Response<ProviderDto>>(createContent, _jsonOptions);

        // Act
        var response = await _client.GetAsync($"/api/v1/providers/{createdProvider!.Data!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Response<ProviderDto>>(content, _jsonOptions);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("444555666", result.Data.Nit);
        Assert.Equal("Proveedor Por ID", result.Data.Name);
    }

    [Fact]
    public async Task GetProviderById_WhenProviderDoesNotExist_ShouldReturnBadRequest()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/v1/providers/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetProviderByNit_WhenProviderExists_ShouldReturnProvider()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        
        var createCommand = new CreateProviderCommand
        {
            Nit = "777888999",
            Name = "Proveedor Por NIT",
            Email = "pornit@test.com"
        };
        await _client.PostAsJsonAsync("/api/v1/providers", createCommand);

        // Act
        var response = await _client.GetAsync("/api/v1/providers/by-nit/777888999");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Response<ProviderDto>>(content, _jsonOptions);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("777888999", result.Data.Nit);
    }

    [Fact]
    public async Task GetProviderByNit_WhenProviderDoesNotExist_ShouldReturnBadRequest()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);

        // Act
        var response = await _client.GetAsync("/api/v1/providers/by-nit/000000000");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAllProviders_WithNitFilter_ShouldReturnFilteredProviders()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        
        await _client.PostAsJsonAsync("/api/v1/providers", new CreateProviderCommand
        {
            Nit = "111000111",
            Name = "Proveedor Filtro 1",
            Email = "filtro1@test.com"
        });

        // Act
        var response = await _client.GetAsync("/api/v1/providers?nit=111000111");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponsePagination<IEnumerable<ProviderDto>>>(content, _jsonOptions);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.True(result.Data.All(p => p.Nit.Contains("111000111")));
    }

    [Fact]
    public async Task GetAllProviders_WithSearchTerm_ShouldReturnFilteredProviders()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        
        await _client.PostAsJsonAsync("/api/v1/providers", new CreateProviderCommand
        {
            Nit = "222000222",
            Name = "Empresa Tecnológica ABC",
            Email = "abc@tech.com"
        });

        // Act
        var response = await _client.GetAsync("/api/v1/providers?searchTerm=Tecnológica");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponsePagination<IEnumerable<ProviderDto>>>(content, _jsonOptions);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }

    #endregion

    #region Update Tests (PUT)

    [Fact]
    public async Task UpdateProvider_WhenProviderDoesNotExist_ShouldReturnBadRequest()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        var nonExistentId = Guid.NewGuid();

        var updateCommand = new UpdateProviderCommand
        {
            Id = nonExistentId,
            Nit = "999999999",
            Name = "Proveedor Inexistente",
            Email = "inexistente@test.com"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/providers/{nonExistentId}", updateCommand);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProvider_WhenIdIsEmpty_ShouldReturnBadRequest()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);

        var updateCommand = new UpdateProviderCommand
        {
            Nit = "123456789",
            Name = "Proveedor",
            Email = "test@test.com"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/providers/{Guid.Empty}", updateCommand);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProvider_WhenEmailIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        
        var createCommand = new CreateProviderCommand
        {
            Nit = "666777888",
            Name = "Proveedor Para Update",
            Email = "update@test.com"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/v1/providers", createCommand);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdProvider = JsonSerializer.Deserialize<Response<ProviderDto>>(createContent, _jsonOptions);

        var updateCommand = new UpdateProviderCommand
        {
            Id = createdProvider!.Data!.Id,
            Nit = "666777888",
            Name = "Proveedor",
            Email = "email-invalido"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/providers/{createdProvider.Data.Id}", updateCommand);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Delete Tests (DELETE)

    [Fact]
    public async Task DeleteProvider_WhenProviderExists_ShouldDeleteProvider()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        
        var createCommand = new CreateProviderCommand
        {
            Nit = "555666777",
            Name = "Proveedor Para Eliminar",
            Email = "eliminar@test.com"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/v1/providers", createCommand);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdProvider = JsonSerializer.Deserialize<Response<ProviderDto>>(createContent, _jsonOptions);

        // Act
        var response = await _client.DeleteAsync($"/api/v1/providers/{createdProvider!.Data!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/v1/providers/{createdProvider.Data.Id}");
        Assert.Equal(HttpStatusCode.BadRequest, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteProvider_WhenProviderDoesNotExist_ShouldReturnBadRequest()
    {
        // Arrange
        await AuthHelper.GetAuthenticatedClientAsync(_client);
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/v1/providers/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion
}

/// <summary>
/// Class to deserialize paginated results (matches ResponsePagination from API)
/// </summary>
public class ResponsePagination<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
