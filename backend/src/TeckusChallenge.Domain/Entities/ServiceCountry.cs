namespace TekusChallenge.Domain.Entities;

/// <summary>
/// Junction table for many-to-many relationship between Services and Countries
/// </summary>
public class ServiceCountry : BaseEntity
{
    /// <summary>
    /// Service foreign key
    /// </summary>
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Navigation property to Service
    /// </summary>
    public virtual Service Service { get; set; } = null!;

    /// <summary>
    /// Country foreign key (ISO Alpha-2 code)
    /// References Country.Code instead of a GUID
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to Country
    /// </summary>
    public virtual Country Country { get; set; } = null!;
}

