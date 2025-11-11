using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Statistics.Queries.GetProvidersByCountry;

/// <summary>
/// Query to get the count of providers grouped by country
/// </summary>
public sealed record GetProvidersByCountryQuery : IRequest<Response<ProvidersStatisticsDto>>
{
}

