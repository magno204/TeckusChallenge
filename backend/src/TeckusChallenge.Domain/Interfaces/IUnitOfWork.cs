namespace TekusChallenge.Domain.Interfaces;

/// <summary>
/// Unit of Work interface for managing database transactions
/// Ensures all repository operations are committed together (ACID principle)
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Provider repository
    /// </summary>
    IProviderRepository Providers { get; }

    /// <summary>
    /// Service repository
    /// </summary>
    IServiceRepository Services { get; }

    /// <summary>
    /// Country repository
    /// </summary>
    ICountryRepository Countries { get; }

    /// <summary>
    /// Provider custom field repository
    /// </summary>
    IProviderCustomFieldRepository ProviderCustomFields { get; }

    /// <summary>
    /// Saves all pending changes to the database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}

