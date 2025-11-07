namespace TeckusChallenge.Domain.Entities;

/// <summary>
/// Provider entity representing a service provider
/// </summary>
public class Provider : BaseEntity
{
    /// <summary>
    /// Tax identification number (NIT)
    /// </summary>
    public string Nit { get; set; } = string.Empty;

    /// <summary>
    /// Provider name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Provider email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to services offered by this provider (One-to-Many relationship)
    /// A provider can offer multiple services
    /// </summary>
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    /// <summary>
    /// Navigation property to custom fields for this provider
    /// </summary>
    public virtual ICollection<ProviderCustomField> CustomFields { get; set; } = new List<ProviderCustomField>();
}

