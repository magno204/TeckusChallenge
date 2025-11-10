using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekusChallenge.Domain.Entities;

namespace TekusChallenge.Infrastructure.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.HourlyRate)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.ProviderId)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(s => s.CreatedBy)
            .HasMaxLength(100);

        builder.Property(s => s.UpdatedAt)
            .IsRequired(false);

        builder.Property(s => s.UpdatedBy)
            .HasMaxLength(100);

        builder.HasIndex(s => s.ProviderId)
            .HasDatabaseName("IX_Services_ProviderId");

        builder.HasIndex(s => s.Name)
            .HasDatabaseName("IX_Services_Name");

        builder.HasIndex(s => s.HourlyRate)
            .HasDatabaseName("IX_Services_HourlyRate");

        builder.HasOne(s => s.Provider)
            .WithMany(p => p.Services)
            .HasForeignKey(s => s.ProviderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Services_Providers");

        builder.HasMany(s => s.ServiceCountries)
            .WithOne(sc => sc.Service)
            .HasForeignKey(sc => sc.ServiceId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ServiceCountries_Services");
    }
}

