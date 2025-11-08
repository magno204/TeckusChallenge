using Microsoft.EntityFrameworkCore;
using TeckusChallenge.Domain.Entities;
using TeckusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Data;

namespace TekusChallenge.Infrastructure.Repositories;

/// <summary>
/// Provider custom field repository implementation with custom field-specific operations
/// Extends GenericRepository for common operations and adds custom queries
/// </summary>
public class ProviderCustomFieldRepository : GenericRepository<ProviderCustomField>, IProviderCustomFieldRepository
{
    public ProviderCustomFieldRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<ProviderCustomField?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(pcf => pcf.Provider)
            .FirstOrDefaultAsync(pcf => pcf.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<ProviderCustomField>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(pcf => pcf.Provider)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProviderCustomField>> GetByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default)
    {
        if (providerId == Guid.Empty)
        {
            throw new ArgumentException("Provider ID cannot be empty", nameof(providerId));
        }

        return await _dbSet
            .Where(pcf => pcf.ProviderId == providerId)
            .OrderBy(pcf => pcf.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProviderCustomField?> GetByProviderIdAndFieldNameAsync(Guid providerId, string fieldName, CancellationToken cancellationToken = default)
    {
        if (providerId == Guid.Empty)
        {
            throw new ArgumentException("Provider ID cannot be empty", nameof(providerId));
        }

        if (string.IsNullOrWhiteSpace(fieldName))
        {
            throw new ArgumentException("Field name cannot be null or empty", nameof(fieldName));
        }

        return await _dbSet
            .FirstOrDefaultAsync(pcf => pcf.ProviderId == providerId && pcf.FieldName == fieldName, cancellationToken);
    }

    public async Task DeleteByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default)
    {
        if (providerId == Guid.Empty)
        {
            throw new ArgumentException("Provider ID cannot be empty", nameof(providerId));
        }

        var customFields = await _dbSet
            .Where(pcf => pcf.ProviderId == providerId)
            .ToListAsync(cancellationToken);

        if (customFields.Any())
        {
            _dbSet.RemoveRange(customFields);
        }
    }
}

