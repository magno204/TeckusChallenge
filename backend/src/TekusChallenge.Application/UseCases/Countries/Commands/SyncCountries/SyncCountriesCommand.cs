using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Countries.Commands.SyncCountries;

public sealed record SyncCountriesCommand : IRequest<Response<int>>
{
    public IEnumerable<string> CountryCodes { get; init; } = Array.Empty<string>();

    public Dictionary<string, CountryDto> CountriesFromApi { get; init; } = new();
}
