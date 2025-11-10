using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Data;

namespace TekusChallenge.Infrastructure.Repositories;

/// <summary>
/// Country repository implementation for country-specific operations
/// </summary>
public class CountryRepository : ICountryRepository
{
    private readonly ApplicationDbContext _context;

    public CountryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Country> AddAsync(Country entity, CancellationToken cancellationToken = default)
    {
        entity.Code = entity.Code.ToUpper();
        entity.CodeAlpha3 = entity.CodeAlpha3.ToUpper();
        
        await _context.Countries.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Countries.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Country code cannot be null or empty", nameof(code));
        }

        return await _context.Countries.AnyAsync(c => c.Code == code.ToUpper(), cancellationToken);
    }

    public async Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Countries
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Country?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Country code cannot be null or empty", nameof(code));
        }

        return await _context.Countries
            .Include(c => c.ServiceCountries)
                .ThenInclude(sc => sc.Service)
            .FirstOrDefaultAsync(c => c.Code == code.ToUpper(), cancellationToken);
    }

    public async Task<(IEnumerable<Country> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Country> query = _context.Countries;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerSearchTerm = searchTerm.ToLower();
            query = query.Where(c => 
                c.Name.ToLower().Contains(lowerSearchTerm) ||
                c.Code.ToLower().Contains(lowerSearchTerm) ||
                c.CodeAlpha3.ToLower().Contains(lowerSearchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<bool> UpdateAsync(Country entity, CancellationToken cancellationToken = default)
    {
        var existingCountry = await _context.Countries
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Code == entity.Code, cancellationToken);

        if (existingCountry == null)
        {
            return false;
        }

        entity.Code = entity.Code.ToUpper();
        entity.CodeAlpha3 = entity.CodeAlpha3.ToUpper();

        _context.Countries.Update(entity);
        return await Task.FromResult(true);
    }
}

