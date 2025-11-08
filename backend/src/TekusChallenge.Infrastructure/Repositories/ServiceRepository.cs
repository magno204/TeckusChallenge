using Microsoft.EntityFrameworkCore;
using TeckusChallenge.Domain.Entities;
using TeckusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Data;

namespace TekusChallenge.Infrastructure.Repositories;

/// <summary>
/// Service repository implementation with service-specific operations
/// Extends GenericRepository for common operations and adds custom queries
/// </summary>
public class ServiceRepository : GenericRepository<Service>, IServiceRepository
{
    public ServiceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Provider)
            .Include(s => s.ServiceCountries)
                .ThenInclude(sc => sc.Country)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Provider)
            .Include(s => s.ServiceCountries)
                .ThenInclude(sc => sc.Country)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Service>> GetByProviderIdAsync(
        Guid providerId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Provider)
            .Include(s => s.ServiceCountries)
                .ThenInclude(sc => sc.Country)
            .Where(s => s.ProviderId == providerId)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Service>> GetByCountryIdAsync(
        Guid countryId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Provider)
            .Include(s => s.ServiceCountries)
                .ThenInclude(sc => sc.Country)
            .Where(s => s.ServiceCountries.Any(sc => sc.CountryCode == countryId.ToString()))
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }
}

