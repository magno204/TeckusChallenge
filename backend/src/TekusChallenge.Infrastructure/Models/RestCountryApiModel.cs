using System.Text.Json.Serialization;

namespace TekusChallenge.Infrastructure.Models;

/// <summary>
/// Model representing a country from the REST Countries API
/// https://restcountries.com/v3.1/all
/// </summary>
public class RestCountryApiModel
{
    /// <summary>
    /// ISO Alpha-2 country code (e.g.: "SY", "CO")
    /// </summary>
    [JsonPropertyName("cca2")]
    public string? Cca2 { get; set; }

    /// <summary>
    /// ISO Alpha-3 country code (e.g.: "SYR", "COL")
    /// </summary>
    [JsonPropertyName("cca3")]
    public string? Cca3 { get; set; }

    /// <summary>
    /// Country name information
    /// </summary>
    [JsonPropertyName("name")]
    public NameInfo? Name { get; set; }

    /// <summary>
    /// Country flag information
    /// </summary>
    [JsonPropertyName("flags")]
    public FlagInfo? Flags { get; set; }
}
