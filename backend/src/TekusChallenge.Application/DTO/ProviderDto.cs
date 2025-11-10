namespace TekusChallenge.Application.DTO;

public class ProviderDto
{
    public Guid Id { get; set; }

    public string Nit { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public List<ProviderCustomFieldDto> CustomFields { get; set; } = new();

    public List<ServiceDto> Services { get; set; } = new();
}

