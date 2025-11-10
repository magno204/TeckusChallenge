using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Models;

namespace TekusChallenge.Infrastructure.Services;

public class RestCountriesService : IRestCountriesService
{
    private readonly HttpClient _httpClient;
    private readonly string _allCountriesEndpoint;
    private readonly string _countryByCodeEndpoint;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public RestCountriesService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        
        var baseUrl = configuration["ExternalApis:RestCountriesBaseUrl"] ?? "https://restcountries.com/v3.1/";
        _httpClient.BaseAddress = new Uri(baseUrl);
        
        _allCountriesEndpoint = configuration["ExternalApis:RestCountriesAllCountriesEndpoint"] ?? "all?fields=name,flags,cca2,cca3";
        _countryByCodeEndpoint = configuration["ExternalApis:RestCountriesCountryByCodeEndpoint"] ?? "alpha/{code}?fields=name,flags,cca2,cca3";
    }

    public async Task<IEnumerable<Country>> FetchAllCountriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(_allCountriesEndpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var countries = JsonSerializer.Deserialize<List<RestCountryApiModel>>(content, JsonOptions);

            if (countries == null || !countries.Any())
            {
                return Enumerable.Empty<Country>();
            }

            var mappedCountries = countries.Select(MapToEntity).Where(c => !string.IsNullOrWhiteSpace(c.Code)).ToList();

            return mappedCountries;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Could not connect to external countries API", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("External API response does not have the expected format", ex);
        }
    }

    public async Task<Country?> FetchCountryByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        try
        {
            var endpoint = _countryByCodeEndpoint.Replace("{code}", code);
            var response = await _httpClient.GetAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var country = JsonSerializer.Deserialize<RestCountryApiModel>(content, JsonOptions);
            
            return country != null ? MapToEntity(country) : null;
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private Country MapToEntity(RestCountryApiModel apiModel)
    {
        return new Country
        {
            Code = apiModel.Cca2?.ToUpper() ?? string.Empty,
            CodeAlpha3 = apiModel.Cca3?.ToUpper() ?? string.Empty,
            Name = apiModel.Name?.Common ?? string.Empty,
            Flag = apiModel.Flags?.Png
        };
    }
}