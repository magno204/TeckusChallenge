using TekusChallenge.Domain.Entities;

namespace TekusChallenge.Domain.Interfaces;

public interface IRestCountriesService
{
    Task<IEnumerable<Country>> FetchAllCountriesAsync(CancellationToken cancellationToken = default);
    
    Task<Country?> FetchCountryByCodeAsync(string code, CancellationToken cancellationToken = default);
}