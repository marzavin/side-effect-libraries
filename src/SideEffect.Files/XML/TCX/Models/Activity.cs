using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Information about activity.
/// </summary>
public class Activity : XmlFileNode
{
    /// <summary>
    /// Gets or sets activity identifier value (DateTime).
    /// </summary>
    public DateTime Id { get; set; }

    /// <summary>
    /// Gets or sets activity sport type.
    /// </summary>
    public string Sport { get; set; }

    /// <summary>
    /// Gets or sets training details.
    /// </summary>
    public Training Training { get; set; }

    /// <summary>
    /// Gets or sets creator info.
    /// </summary>
    public Creator Creator { get; set; }

    /// <summary>
    /// Gets or sets list of laps.
    /// </summary>
    public List<Lap> Laps { get; set; }

    internal override void Load(XElement xmlElement)
    {
        Id = ParseUniversalTime(xmlElement.GetRequiredElementValue(nameof(Id)));
        Creator = ParseNode<Creator>(xmlElement, nameof(Creator));
        Laps = ParseNodeList<Lap>(xmlElement, nameof(Lap));
        Sport = xmlElement.GetRequiredAttributeValue(nameof(Sport));
        Training = ParseNode<Training>(xmlElement, nameof(Training));        
    }
}
