using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Information about single point of track.
/// </summary>
public class TrackPoint : XmlFileNode
{
    /// <summary>
    /// Gets or sets timestamp of the point.
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Gets or sets altitude in meters.
    /// </summary>
    public double? AltitudeMeters { get; set; }

    /// <summary>
    /// Gets or sets distance in meters.
    /// </summary>
    public double? DistanceMeters { get; set; }

    /// <summary>
    /// Gets or sets heart rate.
    /// </summary>
    public int? HeartRate { get; set; }

    /// <summary>
    /// Gets or sets cadence.
    /// </summary>
    public int? Cadence { get; set; }

    /// <summary>
    /// Gets or sets sensor state.
    /// </summary>
    public string SensorState { get; set; }

    /// <summary>
    /// Gets or sets GPS position of track point.
    /// </summary>
    public Position Position { get; set; }

    internal override void Load(XElement xmlElement)
    {
        AltitudeMeters = ParseNullableDouble(xmlElement.GetOptionalElementValue(nameof(AltitudeMeters)));
        Cadence = ParseNullableInteger(xmlElement.GetOptionalElementValue(nameof(Cadence)));
        DistanceMeters = ParseNullableDouble(xmlElement.GetOptionalElementValue(nameof(DistanceMeters)));
        HeartRate = ParseNullableInteger(ParseNodeChildValue(xmlElement, "HeartRateBpm", "Value"));
        Position = ParseNode<Position>(xmlElement, nameof(Position));
        SensorState = xmlElement.GetOptionalElementValue(nameof(SensorState));
        Time = ParseUniversalTime(xmlElement.GetRequiredElementValue(nameof(Time)));
    }
}
