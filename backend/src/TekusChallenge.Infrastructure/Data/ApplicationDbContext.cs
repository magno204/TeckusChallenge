using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Infrastructure.Interceptors;

namespace TekusChallenge.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
         AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        this._auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<Provider> Providers { get; set; }

    public DbSet<ProviderCustomField> ProviderCustomFields { get; set; }

    public DbSet<Service> Services { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<ServiceCountry> ServiceCountries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

}

