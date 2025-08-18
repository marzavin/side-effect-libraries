using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Information about activity lap.
/// </summary>
public class Lap : XmlFileNode
{
    /// <summary>
    /// Gets or sets cadence.
    /// </summary>
    public int? Cadence { get; set; }

    /// <summary>
    /// Gets or sets calories lost.
    /// </summary>
    public int Calories { get; set; }

    /// <summary>
    /// Gets or sets distance in meters.
    /// </summary>
    public double DistanceMeters { get; set; }

    /// <summary>
    /// Gets or sets intensity.
    /// </summary>
    public string Intensity { get; set; }

    /// <summary>
    /// Gets or sets average heart rate.
    /// </summary>
    public int? AverageHeartRate { get; set; }

    /// <summary>
    /// Gets or sets maximum heart rate.
    /// </summary>
    public int? MaximumHeartRate { get; set; }

    /// <summary>
    /// Gets or sets lap start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets trigger method.
    /// </summary>
    public string TriggerMethod { get; set; }

    /// <summary>
    /// Gets or sets notes.
    /// </summary>
    public string Notes { get; set; }

    /// <summary>
    /// Gets or sets total time in seconds.
    /// </summary>
    public double TotalTimeSeconds { get; set; }

    /// <summary>
    /// Gets or sets maximum speed.
    /// </summary>
    public double? MaximumSpeed { get; set; }

    /// <summary>
    /// Gets or sets lap track.
    /// </summary>
    public List<TrackPoint> Track { get; set; }

    internal override void Load(XElement xmlElement)
    {
        AverageHeartRate = ParseNullableInteger(ParseNodeChildValue(xmlElement, "AverageHeartRateBpm", "Value"));
        MaximumHeartRate = ParseNullableInteger(ParseNodeChildValue(xmlElement, "MaximumHeartRateBpm", "Value"));
        Cadence = ParseNullableInteger(xmlElement.GetOptionalElementValue(nameof(Cadence)));
        Calories = ParseInteger(xmlElement.GetRequiredElementValue(nameof(Calories)));
        DistanceMeters = ParseDouble(xmlElement.GetRequiredElementValue(nameof(DistanceMeters)));
        Intensity = xmlElement.GetRequiredElementValue(nameof(Intensity));
        StartTime = ParseUniversalTime(xmlElement.GetRequiredAttributeValue(nameof(StartTime)));
        Track = ParseNodeChildren<TrackPoint>(xmlElement, nameof(Track), nameof(TrackPoint));
        TriggerMethod = xmlElement.GetRequiredElementValue(nameof(TriggerMethod));
        Notes = xmlElement.GetOptionalElementValue(nameof(Notes));
        TotalTimeSeconds = ParseDouble(xmlElement.GetRequiredElementValue(nameof(TotalTimeSeconds)));
        MaximumSpeed = ParseNullableDouble(xmlElement.GetOptionalElementValue(nameof(MaximumSpeed)));
    }
}
