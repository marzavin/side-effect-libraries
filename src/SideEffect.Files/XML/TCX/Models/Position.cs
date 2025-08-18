using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Information about GPS position.
/// </summary>
public class Position : XmlFileNode
{
    /// <summary>
    /// Gets or sets latitude coordinate.
    /// </summary>
    public double LatitudeDegrees { get; set; }

    /// <summary>
    /// Gets or sets longitude coordinate.
    /// </summary>
    public double LongitudeDegrees { get; set; }

    internal override void Load(XElement xmlElement)
    {
        LatitudeDegrees = ParseDouble(xmlElement.GetRequiredElementValue(nameof(LatitudeDegrees)));
        LongitudeDegrees = ParseDouble(xmlElement.GetRequiredElementValue(nameof(LongitudeDegrees)));
    }
}
