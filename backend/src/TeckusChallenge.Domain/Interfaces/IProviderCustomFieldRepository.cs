using TeckusChallenge.Domain.Entities;

namespace TeckusChallenge.Domain.Interfaces;

/// <summary>
/// Specific repository interface for ProviderCustomField entity
/// Extends generic repository with custom field-specific operations
/// </summary>
public interface IProviderCustomFieldRepository : IGenericRepository<ProviderCustomField>
{
    /// <summary>
    /// Gets all custom fields for a specific provider
    /// </summary>
    Task<IEnumerable<ProviderCustomField>> GetByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific custom field by provider ID and field name
    /// </summary>
    Task<ProviderCustomField?> GetByProviderIdAndFieldNameAsync(Guid providerId, string fieldName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all custom fields for a specific provider
    /// </summary>
    Task DeleteByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default);
}

