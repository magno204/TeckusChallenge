namespace TekusChallenge.Application.DTO;

/// <summary>
/// DTO for providers statistics by country
/// </summary>
public class ProvidersStatisticsDto
{
    /// <summary>
    /// List of providers count grouped by country
    /// </summary>
    public List<CountryStatisticDto> ProvidersByCountry { get; set; } = new();

    /// <summary>
    /// Total number of providers
    /// </summary>
    public int TotalProviders { get; set; }

    /// <summary>
    /// Number of countries where providers offer services
    /// </summary>
    public int TotalCountries { get; set; }
}

/// <summary>
/// Generic country statistic information
/// </summary>
public class CountryStatisticDto
{
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public int Count { get; set; }
}

