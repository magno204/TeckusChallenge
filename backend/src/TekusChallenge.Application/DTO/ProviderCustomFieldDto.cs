namespace TekusChallenge.Application.DTO;

public class ProviderCustomFieldDto
{
    public Guid Id { get; set; }

    public Guid ProviderId { get; set; }

    public string FieldName { get; set; } = string.Empty;

    public string FieldValue { get; set; } = string.Empty;

    public string FieldType { get; set; } = "text";

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

