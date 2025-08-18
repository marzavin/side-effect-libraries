using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Information about build.
/// </summary>
public class Build : XmlFileNode
{
    /// <summary>
    /// Gets or sets build version.
    /// </summary>
    public Version Version { get; set; }

    internal override void Load(XElement xmlElement)
    {
        Version = ParseNode<Version>(xmlElement, nameof(Version));
    }
}
