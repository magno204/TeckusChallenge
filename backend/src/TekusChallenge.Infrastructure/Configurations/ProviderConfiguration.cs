using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekusChallenge.Domain.Entities;

namespace TekusChallenge.Infrastructure.Configurations;

public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable("Providers");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(p => p.Nit)
            .IsRequired()
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(100);

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false);

        builder.Property(p => p.UpdatedBy)
            .HasMaxLength(100);

        builder.HasIndex(p => p.Nit)
            .IsUnique()
            .HasDatabaseName("IX_Providers_Nit");

        builder.HasIndex(p => p.Email)
            .HasDatabaseName("IX_Providers_Email");

        builder.HasIndex(p => p.Name)
            .HasDatabaseName("IX_Providers_Name");

        builder.HasMany(p => p.Services)
            .WithOne(s => s.Provider)
            .HasForeignKey(s => s.ProviderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Services_Providers");

        builder.HasMany(p => p.CustomFields)
            .WithOne(cf => cf.Provider)
            .HasForeignKey(cf => cf.ProviderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ProviderCustomFields_Providers");
    }
}
