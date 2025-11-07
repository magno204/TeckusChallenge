using Microsoft.EntityFrameworkCore;
using TeckusChallenge.Domain.Entities;

namespace TekusChallenge.Infrastructure.Data;

/// <summary>
/// Application database context
/// Manages entity configurations and database operations
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Providers DbSet
    /// </summary>
    public DbSet<Provider> Providers { get; set; }

    /// <summary>
    /// Services DbSet
    /// </summary>
    public DbSet<Service> Services { get; set; }

    /// <summary>
    /// Countries DbSet
    /// </summary>
    public DbSet<Country> Countries { get; set; }

    /// <summary>
    /// ServiceCountries junction table DbSet
    /// </summary>
    public DbSet<ServiceCountry> ServiceCountries { get; set; }

    /// <summary>
    /// ProviderCustomFields DbSet
    /// </summary>
    public DbSet<ProviderCustomField> ProviderCustomFields { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Automatically update audit fields
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    private void UpdateAuditFields()
    {
        // Update audit fields for entities that inherit from BaseEntity
        var baseEntities = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in baseEntities)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                // TODO: Get current user from authentication context
                // entry.Entity.CreatedBy = _currentUserService.UserId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                // TODO: Get current user from authentication context
                // entry.Entity.UpdatedBy = _currentUserService.UserId;
            }
        }
    }
}

