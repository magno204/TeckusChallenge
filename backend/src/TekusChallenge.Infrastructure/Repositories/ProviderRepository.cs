using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Data;

namespace TekusChallenge.Infrastructure.Repositories;

public class ProviderRepository : IProviderRepository
{
    private readonly ApplicationDbContext _context;

    public ProviderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Provider> AddAsync(Provider entity, CancellationToken cancellationToken = default)
    {
        await _context.Providers.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<int> CountAsync(Expression<Func<Provider, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate == null)
        {
            return await _context.Providers.CountAsync(cancellationToken);
        }

        return await _context.Providers.CountAsync(predicate, cancellationToken);
    }

    public async Task<bool> RemoveAsync(Guid Id)
    {
        try
        {
            var entity = await GetByIdAsync(Id);
            if (entity != null)
            {
                _context.Providers.Remove(entity);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<Provider>> FindAsync(Expression<Func<Provider, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Providers.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<Provider?> FirstOrDefaultAsync(Expression<Func<Provider, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Providers.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<Provider>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Providers
            .Include(p => p.Services)
            .Include(p => p.CustomFields)
            .ToListAsync(cancellationToken);
    }

    public async Task<Provider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Providers
            .Include(p => p.Services)
            .Include(p => p.CustomFields)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Provider?> GetByNitAsync(string nit, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nit))
        {
            throw new ArgumentException("NIT cannot be null or empty", nameof(nit));
        }

        return await _context.Providers
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

        return await _context.Providers
            .Include(p => p.Services)
            .Include(p => p.CustomFields)
            .FirstOrDefaultAsync(p => p.Email.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<(IEnumerable<Provider> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Provider, bool>>? filter = null, Func<IQueryable<Provider>, IOrderedQueryable<Provider>>? orderBy = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Provider> query = _context.Providers;

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

    public async Task<bool> UpdateAsync(Provider entity, CancellationToken cancellationToken = default)
    {
        var existingProvider = await _context.Providers
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == entity.Id, cancellationToken);

        if (existingProvider == null)
        {
            return false;
        }

        existingProvider.Nit = entity.Nit.ToUpper();
        existingProvider.Name = entity.Name.ToUpper();
        existingProvider.Email = entity.Email.ToLower();

        _context.Providers.Update(existingProvider);
        return await Task.FromResult(true);
    }

    public async Task<bool> AnyAsync(Expression<Func<Provider, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Providers.AnyAsync(predicate, cancellationToken);
    }
}

