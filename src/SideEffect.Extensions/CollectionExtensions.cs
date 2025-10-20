namespace SideEffect.Extensions;

/// <summary>
/// A set of extension methods for collections.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Returns 'true' if collection is 'null' or empty (does not contain elements).
    /// </summary>
    /// <param name="collection">Source collection.</param>
    /// <returns></returns>
    public static bool IsEmpty<T>(this IEnumerable<T> collection)
    {
        return collection == null || !collection.Any();
    }
}