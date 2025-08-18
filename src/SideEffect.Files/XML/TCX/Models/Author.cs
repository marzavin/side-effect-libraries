using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Information about author.
/// </summary>
public class Author : XmlFileNode
{
    /// <summary>
    /// Gets or sets name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets build details.
    /// </summary>
    public Build Build { get; set; }

    /// <summary>
    /// Gets or sets language identifier.
    /// </summary>
    public string LanguageId { get; set; }

    /// <summary>
    /// Gets or sets part number.
    /// </summary>
    public string PartNumber { get; set; }

    internal override void Load(XElement xmlElement)
    {
        LanguageId = xmlElement.GetRequiredElementValue("LangID");
        Name = xmlElement.GetRequiredElementValue(nameof(Name));
        PartNumber = xmlElement.GetRequiredElementValue(nameof(PartNumber));
        Build = ParseNode<Build>(xmlElement, nameof(Build));
    }
}
