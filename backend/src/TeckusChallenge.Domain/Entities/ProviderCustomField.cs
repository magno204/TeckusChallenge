namespace TekusChallenge.Domain.Entities;

/// <summary>
/// Custom fields that can be added dynamically to providers
/// Example: "Contact number on Mars", "Number of pets on payroll"
/// </summary>
public class ProviderCustomField : BaseEntity
{
    /// <summary>
    /// Provider foreign key
    /// </summary>
    public Guid ProviderId { get; set; }

    /// <summary>
    /// Navigation property to Provider
    /// </summary>
    public virtual Provider Provider { get; set; } = null!;

    /// <summary>
    /// Custom field name/label
    /// </summary>
    public string FieldName { get; set; } = string.Empty;

    /// <summary>
    /// Custom field value (stored as string for flexibility)
    /// </summary>
    public string FieldValue { get; set; } = string.Empty;

    /// <summary>
    /// Field data type (text, number, date, boolean, etc.)
    /// </summary>
    public string FieldType { get; set; } = "text";

    /// <summary>
    /// Optional description of what this field represents
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Display order for UI purposes
    /// </summary>
    public int DisplayOrder { get; set; } = 0;
}

