using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Countries.Queries.GetAllCountries;

public sealed record GetAllCountriesQuery : IRequest<Response<IEnumerable<CountryDto>>>
{
}
