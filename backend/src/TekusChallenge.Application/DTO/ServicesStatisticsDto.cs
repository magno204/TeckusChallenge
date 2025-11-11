namespace TekusChallenge.Application.DTO;

/// <summary>
/// DTO for services statistics by country
/// </summary>
public class ServicesStatisticsDto
{
    /// <summary>
    /// List of services count grouped by country
    /// </summary>
    public List<CountryStatisticDto> ServicesByCountry { get; set; } = new();

    /// <summary>
    /// Total number of services
    /// </summary>
    public int TotalServices { get; set; }

    /// <summary>
    /// Number of countries where services are offered
    /// </summary>
    public int TotalCountries { get; set; }

    /// <summary>
    /// Average hourly rate (USD)
    /// </summary>
    public decimal AverageHourlyRate { get; set; }
}

