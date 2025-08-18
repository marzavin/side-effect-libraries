using System.Xml.Linq;

namespace SideEffect.Files.XML;

/// <summary>
/// Extensible Markup Language (XML) file.
/// </summary>
public abstract class ExtensibleMarkupLanguageFile<TData> : FileFormatBase<TData>
{
    /// <summary>
    /// Gets list of supported file extensions.
    /// </summary>
    public override IReadOnlyList<string> SupportedExtensions { get; } = ["xml"];

    /// <inheritdoc />
    protected override async Task<TData> LoadContentFromStreamAsync(byte[] content, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(content);
        using var reader = new StreamReader(stream);

        var xmlString = await reader.ReadToEndAsync(cancellationToken);

        reader.Close();
        stream.Close();

        var xmlDocument = XDocument.Parse(xmlString.Trim());

        return await ParseFileContentAsync(xmlDocument, cancellationToken);
    }

    /// <summary>
    /// Parses content of the XML file.
    /// </summary>
    /// <param name="xmlDocument">Content of the file as <see cref="XDocument"/>.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>Parsed content of the file.</returns>
    protected abstract Task<TData> ParseFileContentAsync(XDocument xmlDocument, CancellationToken cancellationToken = default);
}
