using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekusChallenge.Domain.Entities;

namespace TekusChallenge.Infrastructure.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");

        builder.HasKey(c => c.Code);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(2)
            .IsUnicode(false);

        builder.Property(c => c.CodeAlpha3)
            .IsRequired()
            .HasMaxLength(3)
            .IsUnicode(false);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Flag)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.HasIndex(c => c.CodeAlpha3)
            .HasDatabaseName("IX_Countries_CodeAlpha3");

        builder.HasIndex(c => c.Name)
            .HasDatabaseName("IX_Countries_Name");

        builder.HasMany(c => c.ServiceCountries)
            .WithOne(sc => sc.Country)
            .HasForeignKey(sc => sc.CountryCode)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_ServiceCountries_Countries");
    }
}

