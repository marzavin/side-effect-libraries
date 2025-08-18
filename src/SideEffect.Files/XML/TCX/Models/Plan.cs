using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Information about training plan.
/// </summary>
public class Plan : XmlFileNode
{
    /// <summary>
    /// Gets or sets training type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets flag if training is interval workout.
    /// </summary>
    public bool IntervalWorkout { get; set; }

    internal override void Load(XElement xmlElement)
    {
        IntervalWorkout = ParseBoolean(xmlElement.GetRequiredAttributeValue(nameof(IntervalWorkout)));
        Type = xmlElement.GetRequiredAttributeValue(nameof(Type));
    }
}
