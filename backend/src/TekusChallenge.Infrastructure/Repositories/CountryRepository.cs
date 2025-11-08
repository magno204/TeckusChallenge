using Microsoft.EntityFrameworkCore;
using TeckusChallenge.Domain.Entities;
using TeckusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Data;

namespace TekusChallenge.Infrastructure.Repositories;

/// <summary>
/// Country repository implementation for country-specific operations
/// </summary>
public class CountryRepository : ICountryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Country> _dbSet;

    public CountryRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Country>();
    }

    public async Task<Country?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Country code cannot be null or empty", nameof(code));
        }

        return await _dbSet
            .Include(c => c.ServiceCountries)
                .ThenInclude(sc => sc.Service)
            .FirstOrDefaultAsync(c => c.Code == code.ToUpper(), cancellationToken);
    }

    public async Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Country> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

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

    public async Task<Country> AddAsync(Country country, CancellationToken cancellationToken = default)
    {
        if (country == null)
        {
            throw new ArgumentNullException(nameof(country));
        }

        country.Code = country.Code.ToUpper();
        country.CodeAlpha3 = country.CodeAlpha3.ToUpper();

        await _dbSet.AddAsync(country, cancellationToken);
        return country;
    }

    public void UpdateAsync(Country country)
    {
        if (country == null)
        {
            throw new ArgumentNullException(nameof(country));
        }

        country.Code = country.Code.ToUpper();
        country.CodeAlpha3 = country.CodeAlpha3.ToUpper();

        _dbSet.Update(country);
    }

    public async Task<bool> ExistsAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Country code cannot be null or empty", nameof(code));
        }

        return await _dbSet.AnyAsync(c => c.Code == code.ToUpper(), cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }
}

