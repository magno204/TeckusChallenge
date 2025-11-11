namespace TekusChallenge.Application.DTO;

/// <summary>
/// DTO for the summary report with all key indicators
/// </summary>
public class SummaryReportDto
{
    /// <summary>
    /// Statistics grouped by country
    /// </summary>
    public List<StatisticsByCountryDto> CountryStatistics { get; set; } = new();

    /// <summary>
    /// Total number of providers in the system
    /// </summary>
    public int TotalProviders { get; set; }

    /// <summary>
    /// Total number of services in the system
    /// </summary>
    public int TotalServices { get; set; }

    /// <summary>
    /// Total number of countries where services are offered
    /// </summary>
    public int TotalCountriesCovered { get; set; }

    /// <summary>
    /// Average hourly rate across all services (in USD)
    /// </summary>
    public decimal AverageHourlyRate { get; set; }

    /// <summary>
    /// Service with the highest hourly rate
    /// </summary>
    public ServiceSummaryDto? MostExpensiveService { get; set; }

    /// <summary>
    /// Service with the lowest hourly rate
    /// </summary>
    public ServiceSummaryDto? CheapestService { get; set; }
}

/// <summary>
/// Simplified service information for summary reports
/// </summary>
public class ServiceSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string ProviderName { get; set; } = string.Empty;
}

