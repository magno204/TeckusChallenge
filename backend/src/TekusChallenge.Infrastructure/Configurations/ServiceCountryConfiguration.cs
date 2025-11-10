using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekusChallenge.Domain.Entities;

namespace TekusChallenge.Infrastructure.Configurations;

public class ServiceCountryConfiguration : IEntityTypeConfiguration<ServiceCountry>
{
    public void Configure(EntityTypeBuilder<ServiceCountry> builder)
    {
        builder.ToTable("ServiceCountries");

        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(sc => sc.ServiceId)
            .IsRequired();

        builder.Property(sc => sc.CountryCode)
            .IsRequired()
            .HasMaxLength(2)
            .IsUnicode(false);

        builder.Property(sc => sc.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(sc => sc.CreatedBy)
            .HasMaxLength(100);

        builder.Property(sc => sc.UpdatedAt)
            .IsRequired(false);

        builder.Property(sc => sc.UpdatedBy)
            .HasMaxLength(100);

        builder.HasIndex(sc => sc.ServiceId)
            .HasDatabaseName("IX_ServiceCountries_ServiceId");

        builder.HasIndex(sc => sc.CountryCode)
            .HasDatabaseName("IX_ServiceCountries_CountryCode");

        builder.HasIndex(sc => new { sc.ServiceId, sc.CountryCode })
            .IsUnique()
            .HasDatabaseName("IX_ServiceCountries_ServiceId_CountryCode");

        builder.HasOne(sc => sc.Service)
            .WithMany(s => s.ServiceCountries)
            .HasForeignKey(sc => sc.ServiceId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ServiceCountries_Services");

        builder.HasOne(sc => sc.Country)
            .WithMany(c => c.ServiceCountries)
            .HasForeignKey(sc => sc.CountryCode)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_ServiceCountries_Countries");
    }
}

