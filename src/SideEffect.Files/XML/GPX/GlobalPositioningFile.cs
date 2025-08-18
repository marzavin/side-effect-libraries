using SideEffect.Files.XML.GPX.Models;
using System.Xml.Linq;

namespace SideEffect.Files.XML.GPX;

/// <summary>
/// Global Positioning (GPX) file.
/// </summary>
internal class GlobalPositioningFile : ExtensibleMarkupLanguageFile<Root>
{
    /// <summary>
    /// Gets list of supported file extensions.
    /// </summary>
    public override IReadOnlyList<string> SupportedExtensions { get; } = [ "gpx", "xml" ];

    /// <summary>
    /// Parses content of the XML file.
    /// </summary>
    /// <param name="xmlDocument">Content of the file as <see cref="XDocument"/>.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>Parsed content of the file.</returns>
    protected override Task<Root> ParseFileContentAsync(XDocument xmlDocument, CancellationToken cancellationToken = default)
    {
        var root = new Root();

        root.Load(xmlDocument.Root);

        return Task.FromResult(root);
    }
}
