namespace SideEffect.Extensions;

/// <summary>
/// A set of extension methods for strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns 'true' if string is 'null', empty string or a set of white space characters.
    /// </summary>
    /// <param name="stringValue">String value.</param>
    /// <returns></returns>
    public static bool IsEmpty(this string stringValue)
    {
        return string.IsNullOrWhiteSpace(stringValue);
    }

    /// <summary>
    /// Checks strings for equality without regard to case.
    /// </summary>
    /// <param name="stringValueA">The first string to compare.</param>
    /// <param name="stringValueB">The second string to compare.</param>
    /// <returns>Returns 'true' if the strings are identical regardless of case.</returns>
    public static bool IsSimilar(this string stringValueA, string stringValueB)
    {
        return string.Equals(stringValueA, stringValueB, StringComparison.InvariantCultureIgnoreCase);
    }
}
