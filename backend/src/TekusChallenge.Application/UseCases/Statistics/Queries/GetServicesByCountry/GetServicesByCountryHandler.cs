using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Statistics.Queries.GetServicesByCountry;

/// <summary>
/// Handler for getting the count of services grouped by country
/// </summary>
public class GetServicesByCountryHandler : IRequestHandler<GetServicesByCountryQuery, Response<ServicesStatisticsDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetServicesByCountryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<ServicesStatisticsDto>> Handle(GetServicesByCountryQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<ServicesStatisticsDto>();

        try
        {
            // Get all services with their relationships
            var services = await _unitOfWork.Services.GetAllAsync(cancellationToken);
            var servicesList = services.ToList();

            // Get all countries
            var countries = await _unitOfWork.Countries.GetAllAsync(cancellationToken);
            var countriesList = countries.ToList();

            // Group services by country
            var servicesByCountry = new Dictionary<string, int>();

            foreach (var service in servicesList)
            {
                foreach (var serviceCountry in service.ServiceCountries)
                {
                    var country = countriesList.FirstOrDefault(c => c.Code == serviceCountry.CountryCode);
                    if (country == null) continue;

                    if (!servicesByCountry.ContainsKey(country.Code))
                    {
                        servicesByCountry[country.Code] = 0;
                    }

                    servicesByCountry[country.Code]++;
                }
            }

            // Create statistics list
            var statisticsList = servicesByCountry.Select(kvp =>
            {
                var country = countriesList.First(c => c.Code == kvp.Key);
                return new CountryStatisticDto
                {
                    CountryCode = country.Code,
                    CountryName = country.Name,
                    Count = kvp.Value
                };
            }).OrderByDescending(s => s.Count).ToList();

            // Calculate average hourly rate
            var averageRate = servicesList.Any() 
                ? servicesList.Average(s => s.HourlyRate) 
                : 0;

            var statistics = new ServicesStatisticsDto
            {
                ServicesByCountry = statisticsList,
                TotalServices = servicesList.Count,
                TotalCountries = statisticsList.Count,
                AverageHourlyRate = Math.Round(averageRate, 2)
            };

            response.IsSuccess = true;
            response.Message = $"Found {servicesList.Count} service(s) across {statisticsList.Count} country(ies)";
            response.Data = statistics;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error retrieving services statistics: {ex.Message}";
        }

        return response;
    }
}

