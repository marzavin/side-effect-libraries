namespace SideEffect.Files;

/// <summary>
/// Base class for all file formats.
/// </summary>
public abstract class FileFormatBase<TData>
{
    /// <summary>
    /// Gets list of supported file extensions.
    /// </summary>
    public abstract IReadOnlyList<string> SupportedExtensions { get; }

    /// <summary>
    /// Gets content of the file.
    /// </summary>
    public TData Content { get; private set; }

    /// <summary>
    /// Loads file content.
    /// </summary>
    /// <param name="path">Path to the file.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    public virtual async Task LoadAsync(string path, CancellationToken cancellationToken = default)
    {
        var data = await File.ReadAllBytesAsync(path, cancellationToken);
        await LoadAsync(data, cancellationToken);
    }

    /// <summary>
    /// Loads file content.
    /// </summary>
    /// <param name="data">Content of the file as byte array.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    public virtual async Task LoadAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        Content = await LoadContentFromStreamAsync(data, cancellationToken);
    }

    /// <summary>
    /// Loads file content from stream.
    /// </summary>
    /// <param name="content">Content of the file as byte array.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>Content of the file.</returns>
    protected abstract Task<TData> LoadContentFromStreamAsync(byte[] content, CancellationToken cancellationToken = default);
}
