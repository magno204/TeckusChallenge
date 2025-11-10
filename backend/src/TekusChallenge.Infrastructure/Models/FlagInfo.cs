using System.Text.Json.Serialization;

namespace TekusChallenge.Infrastructure.Models;

/// <summary>
/// Country flag information from REST Countries API
/// </summary>
public class FlagInfo
{
    /// <summary>
    /// Flag URL in PNG format
    /// </summary>
    [JsonPropertyName("png")]
    public string? Png { get; set; }

    /// <summary>
    /// Flag URL in SVG format
    /// </summary>
    [JsonPropertyName("svg")]
    public string? Svg { get; set; }

    /// <summary>
    /// Alternative flag description
    /// </summary>
    [JsonPropertyName("alt")]
    public string? Alt { get; set; }
}
