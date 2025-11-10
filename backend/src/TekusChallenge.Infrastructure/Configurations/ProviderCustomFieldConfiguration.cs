using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekusChallenge.Domain.Entities;

namespace TekusChallenge.Infrastructure.Configurations;

public class ProviderCustomFieldConfiguration : IEntityTypeConfiguration<ProviderCustomField>
{
    public void Configure(EntityTypeBuilder<ProviderCustomField> builder)
    {
        builder.ToTable("ProviderCustomFields");

        builder.HasKey(pcf => pcf.Id);

        builder.Property(pcf => pcf.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(pcf => pcf.ProviderId)
            .IsRequired();

        builder.Property(pcf => pcf.FieldName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pcf => pcf.FieldValue)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(pcf => pcf.FieldType)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("text");

        builder.Property(pcf => pcf.Description)
            .HasMaxLength(500);

        builder.Property(pcf => pcf.DisplayOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(pcf => pcf.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(pcf => pcf.CreatedBy)
            .HasMaxLength(100);

        builder.Property(pcf => pcf.UpdatedAt)
            .IsRequired(false);

        builder.Property(pcf => pcf.UpdatedBy)
            .HasMaxLength(100);

        builder.HasIndex(pcf => pcf.ProviderId)
            .HasDatabaseName("IX_ProviderCustomFields_ProviderId");

        builder.HasIndex(pcf => new { pcf.ProviderId, pcf.FieldName })
            .HasDatabaseName("IX_ProviderCustomFields_ProviderId_FieldName");

        builder.HasOne(pcf => pcf.Provider)
            .WithMany(p => p.CustomFields)
            .HasForeignKey(pcf => pcf.ProviderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ProviderCustomFields_Providers");
    }
}

