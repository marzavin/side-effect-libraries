namespace SideEffect.DataTransfer.Models;

/// <summary>
/// Key-value data transfer object.
/// </summary>
/// <typeparam name="TKey">Type of the key.</typeparam>
/// <typeparam name="TValue">Type of the value.</typeparam>
public class KeyValueModel<TKey, TValue>
{
    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    public TKey Key { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public TValue Value { get; set; }
}
