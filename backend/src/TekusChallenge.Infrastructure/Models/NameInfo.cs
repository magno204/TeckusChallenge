using System.Text.Json.Serialization;

namespace TekusChallenge.Infrastructure.Models;

/// <summary>
/// Country name information from REST Countries API
/// </summary>
public class NameInfo
{
    /// <summary>
    /// Common name of the country (e.g.: "Syria", "Colombia")
    /// </summary>
    [JsonPropertyName("common")]
    public string? Common { get; set; }

    /// <summary>
    /// Official name of the country (e.g.: "Syrian Arab Republic")
    /// </summary>
    [JsonPropertyName("official")]
    public string? Official { get; set; }
}
