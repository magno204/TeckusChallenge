using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Statistics.Queries.GetSummaryReport;

/// <summary>
/// Handler for getting a comprehensive summary report with all key indicators
/// </summary>
public class GetSummaryReportHandler : IRequestHandler<GetSummaryReportQuery, Response<SummaryReportDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSummaryReportHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<SummaryReportDto>> Handle(GetSummaryReportQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<SummaryReportDto>();

        try
        {
            var providers = await _unitOfWork.Providers.GetAllAsync(cancellationToken);
            var providersList = providers.ToList();

            var services = await _unitOfWork.Services.GetAllAsync(cancellationToken);
            var servicesList = services.ToList();

            var countries = await _unitOfWork.Countries.GetAllAsync(cancellationToken);
            var countriesList = countries.ToList();

            var countryStatistics = new Dictionary<string, StatisticsByCountryDto>();

            foreach (var service in servicesList)
            {
                foreach (var serviceCountry in service.ServiceCountries)
                {
                    var country = countriesList.FirstOrDefault(c => c.Code == serviceCountry.CountryCode);
                    if (country == null) continue;

                    if (!countryStatistics.ContainsKey(country.Code))
                    {
                        countryStatistics[country.Code] = new StatisticsByCountryDto
                        {
                            CountryCode = country.Code,
                            CountryName = country.Name,
                            ProvidersCount = 0,
                            ServicesCount = 0
                        };
                    }

                    countryStatistics[country.Code].ServicesCount++;
                    
                    var providersInCountry = servicesList
                        .Where(s => s.ServiceCountries.Any(sc => sc.CountryCode == country.Code))
                        .Select(s => s.ProviderId)
                        .Distinct()
                        .Count();
                    
                    countryStatistics[country.Code].ProvidersCount = providersInCountry;
                }
            }

            var averageRate = servicesList.Any() 
                ? servicesList.Average(s => s.HourlyRate) 
                : 0;

            var mostExpensive = servicesList
                .OrderByDescending(s => s.HourlyRate)
                .Select(s => new ServiceSummaryDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    HourlyRate = s.HourlyRate,
                    ProviderName = s.Provider?.Name ?? "Unknown"
                })
                .FirstOrDefault();

            var cheapest = servicesList
                .OrderBy(s => s.HourlyRate)
                .Select(s => new ServiceSummaryDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    HourlyRate = s.HourlyRate,
                    ProviderName = s.Provider?.Name ?? "Unknown"
                })
                .FirstOrDefault();

            var summaryReport = new SummaryReportDto
            {
                CountryStatistics = countryStatistics.Values.OrderByDescending(c => c.ServicesCount).ToList(),
                TotalProviders = providersList.Count,
                TotalServices = servicesList.Count,
                TotalCountriesCovered = countryStatistics.Count,
                AverageHourlyRate = Math.Round(averageRate, 2),
                MostExpensiveService = mostExpensive,
                CheapestService = cheapest
            };

            response.IsSuccess = true;
            response.Message = "Summary report retrieved successfully";
            response.Data = summaryReport;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error retrieving summary report: {ex.Message}";
        }

        return response;
    }
}

