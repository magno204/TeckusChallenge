using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Statistics.Queries.GetProvidersByCountry;

/// <summary>
/// Handler for getting the count of providers grouped by country
/// </summary>
public class GetProvidersByCountryHandler : IRequestHandler<GetProvidersByCountryQuery, Response<ProvidersStatisticsDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProvidersByCountryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<ProvidersStatisticsDto>> Handle(GetProvidersByCountryQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<ProvidersStatisticsDto>();

        try
        {
            // Get all services with their relationships to get countries
            var services = await _unitOfWork.Services.GetAllAsync(cancellationToken);
            var servicesList = services.ToList();

            // Get all countries
            var countries = await _unitOfWork.Countries.GetAllAsync(cancellationToken);
            var countriesList = countries.ToList();

            // Group providers by country
            var providersByCountry = new Dictionary<string, HashSet<Guid>>();

            foreach (var service in servicesList)
            {
                foreach (var serviceCountry in service.ServiceCountries)
                {
                    var country = countriesList.FirstOrDefault(c => c.Code == serviceCountry.CountryCode);
                    if (country == null) continue;

                    if (!providersByCountry.ContainsKey(country.Code))
                    {
                        providersByCountry[country.Code] = new HashSet<Guid>();
                    }

                    providersByCountry[country.Code].Add(service.ProviderId);
                }
            }

            // Create statistics list
            var statisticsList = providersByCountry.Select(kvp =>
            {
                var country = countriesList.First(c => c.Code == kvp.Key);
                return new CountryStatisticDto
                {
                    CountryCode = country.Code,
                    CountryName = country.Name,
                    Count = kvp.Value.Count
                };
            }).OrderByDescending(s => s.Count).ToList();

            var providers = await _unitOfWork.Providers.GetAllAsync(cancellationToken);
            var totalProviders = providers.Count();

            var statistics = new ProvidersStatisticsDto
            {
                ProvidersByCountry = statisticsList,
                TotalProviders = totalProviders,
                TotalCountries = statisticsList.Count
            };

            response.IsSuccess = true;
            response.Message = $"Found {totalProviders} provider(s) across {statisticsList.Count} country(ies)";
            response.Data = statistics;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error retrieving providers statistics: {ex.Message}";
        }

        return response;
    }
}

