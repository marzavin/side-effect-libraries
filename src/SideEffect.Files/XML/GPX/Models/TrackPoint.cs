using System.Xml.Linq;

namespace SideEffect.Files.XML.GPX.Models;

internal class TrackPoint : XmlFileNode
{
    public double? Altitude { get; set; }

        /// <summary>
    /// Gets or sets latitude coordinate.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Gets or sets longitude coordinate.
    /// </summary>
    public double Longitude { get; set; }

    public DateTime? Time { get; set; }

    internal override void Load(XElement xmlElement)
    {
        Time = ParseUniversalTime(xmlElement.GetOptionalElementValue("time"));
        Altitude = ParseNullableDouble(xmlElement.GetOptionalElementValue("ele"));
        Latitude = ParseDouble(xmlElement.GetRequiredAttributeValue("lat"));
        Longitude = ParseDouble(xmlElement.GetRequiredAttributeValue("lon"));
    }
}
