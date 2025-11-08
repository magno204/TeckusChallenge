using Microsoft.EntityFrameworkCore;
using TeckusChallenge.Domain.Entities;
using TeckusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Data;

namespace TekusChallenge.Infrastructure.Repositories;

/// <summary>
/// Provider repository implementation with provider-specific operations
/// Extends GenericRepository for common operations and adds custom queries
/// </summary>
public class ProviderRepository : GenericRepository<Provider>, IProviderRepository
{
    public ProviderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Provider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Services)
            .Include(p => p.CustomFields)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Provider>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Services)
            .Include(p => p.CustomFields)
            .ToListAsync(cancellationToken);
    }

    public async Task<Provider?> GetByNitAsync(string nit, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nit))
        {
            throw new ArgumentException("NIT cannot be null or empty", nameof(nit));
        }

        return await _dbSet
            .Include(p => p.Services)
            .Include(p => p.CustomFields)
            .FirstOrDefaultAsync(p => p.Nit == nit, cancellationToken);
    }

    public async Task<Provider?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        }

        return await _dbSet
            .Include(p => p.Services)
            .Include(p => p.CustomFields)
            .FirstOrDefaultAsync(p => p.Email.ToLower() == email.ToLower(), cancellationToken);
    }

}

