namespace TekusChallenge.Application.DTO;

public class ServiceDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal HourlyRate { get; set; }

    public string? Description { get; set; }

    public Guid ProviderId { get; set; }

    public string? ProviderName { get; set; }

    public List<CountryDto> Countries { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }
}

