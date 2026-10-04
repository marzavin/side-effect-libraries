namespace SideEffect.Data;

/// <summary>
/// A base entity with a numeric identifier.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Entity identifier.
    /// </summary>
    public int Id { get; set; }
}
