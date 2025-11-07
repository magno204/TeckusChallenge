using TeckusChallenge.Domain.Entities;

namespace TeckusChallenge.Domain.Interfaces;

/// <summary>
/// Specific repository interface for Service entity
/// Extends generic repository with service-specific operations
/// </summary>
public interface IServiceRepository : IGenericRepository<Service>
{
    /// <summary>
    /// Gets all services for a specific provider
    /// </summary>
    Task<IEnumerable<Service>> GetByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all services offered in a specific country
    /// </summary>
    Task<IEnumerable<Service>> GetByCountryIdAsync(Guid countryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches services by name or description
    /// </summary>
    Task<(IEnumerable<Service> Items, int TotalCount)> SearchServicesAsync(
        string searchTerm,
        int pageNumber,
        int pageSize,
        Guid? providerId = null,
        CancellationToken cancellationToken = default);
}

