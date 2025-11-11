using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Statistics.Queries.GetServicesByCountry;

/// <summary>
/// Query to get the count of services grouped by country
/// </summary>
public sealed record GetServicesByCountryQuery : IRequest<Response<ServicesStatisticsDto>>
{
}

