using SideEffect.Files.XML.TCX.Models;
using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX;

/// <summary>
/// Training Center (TCX) file.
/// </summary>
public class TrainingCenterFile : ExtensibleMarkupLanguageFile<TrainingCenterDatabase>
{
    /// <summary>
    /// Gets list of supported file extensions.
    /// </summary>
    public override IReadOnlyList<string> SupportedExtensions { get; } = ["tcx", "xml"];

    /// <summary>
    /// Parses content of the XML file.
    /// </summary>
    /// <param name="xmlDocument">Content of the file as <see cref="XDocument"/>.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>Parsed content of the file.</returns>
    protected override Task<TrainingCenterDatabase> ParseFileContentAsync(XDocument xmlDocument, CancellationToken cancellationToken = default)
    {
        var root = new TrainingCenterDatabase();

        root.Load(xmlDocument.Root);

        return Task.FromResult(root);
    }
}
