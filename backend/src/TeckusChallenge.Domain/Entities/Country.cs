namespace TeckusChallenge.Domain.Entities;

/// <summary>
/// Country entity representing countries where services are offered
/// Data is preloaded from external REST Countries API (https://restcountries.com)
/// Uses ISO Alpha-2 code as natural primary key for stability across API reloads
/// </summary>
public class Country
{
    /// <summary>
    /// ISO Alpha-2 country code (e.g., CO, PE, MX, SY) - Primary Key
    /// This is the natural identifier from the external API
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// ISO Alpha-3 country code (e.g., COL, PER, MEX, SYR)
    /// </summary>
    public string CodeAlpha3 { get; set; } = string.Empty;

    /// <summary>
    /// Country common name (e.g., "Syria", "Colombia")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Flag PNG image URL from REST Countries API
    /// </summary>
    public string? Flag { get; set; }

    /// <summary>
    /// Navigation property to service countries (many-to-many relationship)
    /// </summary>
    public virtual ICollection<ServiceCountry> ServiceCountries { get; set; } = new List<ServiceCountry>();
}

