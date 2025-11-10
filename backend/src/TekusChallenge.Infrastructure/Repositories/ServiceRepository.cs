using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Data;

namespace TekusChallenge.Infrastructure.Repositories;

/// <summary>
/// Service repository implementation with service-specific operations
/// </summary>
public class ServiceRepository : IServiceRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Service> AddAsync(Service entity, CancellationToken cancellationToken = default)
    {
        await _context.Services.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<int> CountAsync(Expression<Func<Service, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate == null)
        {
            return await _context.Services.CountAsync(cancellationToken);
        }

        return await _context.Services.CountAsync(predicate, cancellationToken);
    }

    public async Task<bool> RemoveAsync(Guid Id)
    {
        try
        {
            var entity = await GetByIdAsync(Id);
            if (entity != null)
            {
                _context.Services.Remove(entity);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<Service>> FindAsync(Expression<Func<Service, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Services.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<Service?> FirstOrDefaultAsync(Expression<Func<Service, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Services.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Include(s => s.Provider)
            .Include(s => s.ServiceCountries)
                .ThenInclude(sc => sc.Country)
            .ToListAsync(cancellationToken);
    }

    public async Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Include(s => s.Provider)
            .Include(s => s.ServiceCountries)
                .ThenInclude(sc => sc.Country)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<Service> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Service, bool>>? filter = null, Func<IQueryable<Service>, IOrderedQueryable<Service>>? orderBy = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Service> query = _context.Services;

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

    public async Task<bool> UpdateAsync(Service entity, CancellationToken cancellationToken = default)
    {
        var existingService = await _context.Services
            .Include(s => s.ServiceCountries)
            .SingleOrDefaultAsync(s => s.Id == entity.Id, cancellationToken);

        if (existingService == null)
        {
            return false;
        }

        existingService.Name = entity.Name.ToUpper();
        existingService.Description = entity.Description;
        existingService.HourlyRate = entity.HourlyRate;
        existingService.ProviderId = entity.ProviderId;
        
        existingService.ServiceCountries.Clear();
        foreach (var serviceCountry in entity.ServiceCountries)
        {
            existingService.ServiceCountries.Add(serviceCountry);
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> AnyAsync(Expression<Func<Service, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Services.AnyAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<Service>> GetByProviderIdAsync(
        Guid providerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Services
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
        return await _context.Services
            .Include(s => s.Provider)
            .Include(s => s.ServiceCountries)
                .ThenInclude(sc => sc.Country)
            .Where(s => s.ServiceCountries.Any(sc => sc.CountryCode == countryId.ToString()))
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }
}

