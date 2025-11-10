namespace TekusChallenge.Domain.Entities;

/// <summary>
/// Service entity representing a service offered by a provider
/// </summary>
public class Service : BaseEntity
{
    /// <summary>
    /// Service name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Hourly rate in USD
    /// </summary>
    public decimal HourlyRate { get; set; }

    /// <summary>
    /// Optional service description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Provider foreign key (One-to-Many relationship)
    /// Each service belongs to one provider
    /// </summary>
    public Guid ProviderId { get; set; }

    /// <summary>
    /// Navigation property to the Provider that offers this service
    /// </summary>
    public virtual Provider Provider { get; set; } = null!;

    /// <summary>
    /// Navigation property to service countries (many-to-many relationship)
    /// A service can be offered in multiple countries
    /// </summary>
    public virtual ICollection<ServiceCountry> ServiceCountries { get; set; } = new List<ServiceCountry>();
}

