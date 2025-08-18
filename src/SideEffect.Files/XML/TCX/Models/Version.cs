using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Information about version.
/// </summary>
public class Version : XmlFileNode
{
    /// <summary>
    /// Gets or sets version major.
    /// </summary>
    public int VersionMajor { get; set; }

    /// <summary>
    /// Gets or sets version minor.
    /// </summary>
    public int VersionMinor { get; set; }

    /// <summary>
    /// Gets or sets build major.
    /// </summary>
    public int? BuildMajor { get; set; }

    /// <summary>
    /// Gets or sets build minor.
    /// </summary>
    public int? BuildMinor { get; set; }

    internal override void Load(XElement xmlElement)
    {
        VersionMajor = ParseInteger(xmlElement.GetRequiredElementValue(nameof(VersionMajor)));
        VersionMinor = ParseInteger(xmlElement.GetRequiredElementValue(nameof(VersionMinor)));
        BuildMajor = ParseNullableInteger(xmlElement.GetOptionalElementValue(nameof(BuildMajor)));
        BuildMinor = ParseNullableInteger(xmlElement.GetOptionalElementValue(nameof(BuildMinor)));
    }
}
