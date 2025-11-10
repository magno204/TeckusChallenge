using TekusChallenge.Domain.Entities;

namespace TekusChallenge.Domain.Interfaces;

/// <summary>
/// Repository interface for Country entity
/// Countries are preloaded from external REST Countries API
/// Country uses ISO code as primary key instead of GUID
/// </summary>
public interface ICountryRepository
{
    /// <summary>
    /// Gets a country by its ISO Alpha-2 code (primary key)
    /// </summary>
    Task<Country?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all countries
    /// </summary>
    Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated list of countries
    /// </summary>
    Task<(IEnumerable<Country> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new country
    /// </summary>
    Task<Country> AddAsync(Country country, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing country
    /// </summary>
    Task<bool> UpdateAsync(Country country, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a country code exists
    /// </summary>
    Task<bool> ExistsAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts total countries
    /// </summary>
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}

