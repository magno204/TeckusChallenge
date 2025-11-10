using Microsoft.EntityFrameworkCore.Storage;
using TekusChallenge.Domain.Interfaces;
using TekusChallenge.Infrastructure.Data;

namespace TekusChallenge.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing database transactions and dependencies
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IProviderRepository Providers { get;}

    public IServiceRepository Services { get;}

    public ICountryRepository Countries { get;}

    public IProviderCustomFieldRepository ProviderCustomFields { get;}

    public UnitOfWork(ApplicationDbContext context,
        IProviderRepository providerRepository,
        IServiceRepository serviceRepository,
        ICountryRepository countryRepository,
        IProviderCustomFieldRepository providerCustomFieldRepository)
    {
        _context = context;
        Providers = providerRepository;
        Services = serviceRepository;
        Countries = countryRepository;
        ProviderCustomFields = providerCustomFieldRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }
}

