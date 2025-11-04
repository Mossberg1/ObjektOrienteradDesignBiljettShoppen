namespace Models.Entities.Base;

/// <summary>
/// Basklass för alla objekt som ska lagras i databasen.
/// Alla som ärver från denna klass måste ha en
/// tom konstruktor eftersom Entity Framework Core kräver det.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}