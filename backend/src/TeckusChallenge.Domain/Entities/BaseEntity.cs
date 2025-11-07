namespace TeckusChallenge.Domain.Entities;

/// <summary>
/// Base entity class with common properties for all entities
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier (auto-generated)
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Entity creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Entity creator
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Entity last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Entity updater
    /// </summary>
    public string? UpdatedBy { get; set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
}

