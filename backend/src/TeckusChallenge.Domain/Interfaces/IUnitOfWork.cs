namespace TeckusChallenge.Domain.Interfaces;

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
    /// Saves all pending changes to the database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new database transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

