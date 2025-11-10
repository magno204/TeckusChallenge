namespace TekusChallenge.Application.DTO;

public class ServiceCountryDto
{
    public Guid Id { get; set; }

    public Guid ServiceId { get; set; }

    public string CountryCode { get; set; } = string.Empty;

    public CountryDto? Country { get; set; }

    public DateTime CreatedAt { get; set; }
}

