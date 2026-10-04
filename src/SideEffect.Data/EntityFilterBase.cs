using System.Linq.Expressions;

namespace SideEffect.Data;

/// <summary>
/// A base class for data filters.
/// </summary>
/// <typeparam name="TEntity">Type of entity.</typeparam>
public abstract class EntityFilterBase<TEntity> where TEntity : EntityBase
{
    /// <summary>
    /// Text search string.
    /// </summary>
    public string SearchString { get; set; }

    /// <summary>
    /// Filter by identifier.
    /// </summary>
    public int? IdEq { get; set; }

    /// <summary>
    /// Converts the filter into a search expression.
    /// </summary>
    /// <returns>Search expression.</returns>
    public virtual Expression<Func<TEntity, bool>> ToSearchExpression() 
    {
        return entity => (IdEq == null || IdEq == entity.Id);
    }
}
