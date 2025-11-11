namespace TekusChallenge.Application.DTO;

/// <summary>
/// DTO for statistics grouped by country
/// </summary>
public class StatisticsByCountryDto
{
    /// <summary>
    /// Country ISO code (e.g., "CO", "PE", "MX")
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// Country name (e.g., "Colombia", "Peru", "Mexico")
    /// </summary>
    public string CountryName { get; set; } = string.Empty;

    /// <summary>
    /// Number of providers offering services in this country
    /// </summary>
    public int ProvidersCount { get; set; }

    /// <summary>
    /// Number of services available in this country
    /// </summary>
    public int ServicesCount { get; set; }
}

