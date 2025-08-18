using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Information about creator.
/// </summary>
public class Creator : XmlFileNode
{
    /// <summary>
    /// Gets or sets creator name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets version.
    /// </summary>
    public Version Version { get; set; }

    /// <summary>
    /// Gets or sets unit identifier.
    /// </summary>
    public int UnitId { get; set; }

    /// <summary>
    /// Gets or sets product identifier.
    /// </summary>
    public int ProductId { get; set; }

    internal override void Load(XElement xmlElement)
    {
        Name = xmlElement.GetRequiredElementValue(nameof(Name));
        ProductId = ParseInteger(xmlElement.GetRequiredElementValue("ProductID"));
        UnitId = ParseInteger(xmlElement.GetRequiredElementValue(nameof(UnitId)));
        Version = ParseNode<Version>(xmlElement, nameof(Version));
    }
}