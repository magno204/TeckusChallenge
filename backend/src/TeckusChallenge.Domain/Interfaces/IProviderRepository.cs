using TekusChallenge.Domain.Entities;

namespace TekusChallenge.Domain.Interfaces;

/// <summary>
/// Specific repository interface for Provider entity
/// Extends generic repository with provider-specific operations
/// </summary>
public interface IProviderRepository : IGenericRepository<Provider>
{
    /// <summary>
    /// Gets a provider by NIT
    /// </summary>
    Task<Provider?> GetByNitAsync(string nit, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a provider by email
    /// </summary>
    Task<Provider?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

}

