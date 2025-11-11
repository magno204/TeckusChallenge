using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Data;

namespace TekusChallenge.Infrastructure.Repositories;

/// <summary>
/// Provider custom field repository implementation with custom field-specific operations
/// </summary>
public class ProviderCustomFieldRepository : IProviderCustomFieldRepository
{
    private readonly ApplicationDbContext _context;

    public ProviderCustomFieldRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProviderCustomField> AddAsync(ProviderCustomField entity, CancellationToken cancellationToken = default)
    {
        await _context.ProviderCustomFields.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<int> CountAsync(Expression<Func<ProviderCustomField, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate == null)
        {
            return await _context.ProviderCustomFields.CountAsync(cancellationToken);
        }

        return await _context.ProviderCustomFields.CountAsync(predicate, cancellationToken);
    }

    public async Task<bool> RemoveAsync(Guid Id)
    {
        try
        {
            var entity = await GetByIdAsync(Id);
            if(entity == null)
            {
                return false;
            }
            _context.ProviderCustomFields.Remove(entity);
            return await Task.FromResult(true);
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<ProviderCustomField>> FindAsync(Expression<Func<ProviderCustomField, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ProviderCustomFields.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<ProviderCustomField?> FirstOrDefaultAsync(Expression<Func<ProviderCustomField, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ProviderCustomFields
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<ProviderCustomField>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProviderCustomFields
            .Include(pcf => pcf.Provider)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProviderCustomField?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProviderCustomFields
            .Include(pcf => pcf.Provider)
            .FirstOrDefaultAsync(pcf => pcf.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<ProviderCustomField> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<ProviderCustomField, bool>>? filter = null, Func<IQueryable<ProviderCustomField>, IOrderedQueryable<ProviderCustomField>>? orderBy = null, CancellationToken cancellationToken = default)
    {
        IQueryable<ProviderCustomField> query = _context.ProviderCustomFields;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = orderBy != null 
            ? orderBy(query) 
            : query.OrderByDescending(x => x.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<bool> UpdateAsync(ProviderCustomField entity, CancellationToken cancellationToken = default)
    {
        var exists = await _context.ProviderCustomFields
            .AsNoTracking()
            .AnyAsync(pcf => pcf.Id == entity.Id, cancellationToken);

        if (!exists)
        {
            return false;
        }

        entity.FieldName = entity.FieldName.ToUpper();
        entity.Description = string.IsNullOrWhiteSpace(entity.Description) ? null : entity.Description;

        _context.ProviderCustomFields.Update(entity);

        return await Task.FromResult(true);
    }

    public async Task<bool> AnyAsync(Expression<Func<ProviderCustomField, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ProviderCustomFields.AnyAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<ProviderCustomField>> GetByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default)
    {
        if (providerId == Guid.Empty)
        {
            throw new ArgumentException("Provider ID cannot be empty", nameof(providerId));
        }

        return await _context.ProviderCustomFields
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

        return await _context.ProviderCustomFields
            .FirstOrDefaultAsync(pcf => pcf.ProviderId == providerId && pcf.FieldName == fieldName, cancellationToken);
    }

    public async Task DeleteByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default)
    {
        if (providerId == Guid.Empty)
        {
            throw new ArgumentException("Provider ID cannot be empty", nameof(providerId));
        }

        var customFields = await _context.ProviderCustomFields
            .Where(pcf => pcf.ProviderId == providerId)
            .ToListAsync(cancellationToken);

        if (customFields.Any())
        {
            _context.ProviderCustomFields.RemoveRange(customFields);
        }
    }
}

