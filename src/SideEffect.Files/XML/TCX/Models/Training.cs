using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Information about training.
/// </summary>
public class Training : XmlFileNode
{
    /// <summary>
    /// Gets or sets information about training plan.
    /// </summary>
    public Plan Plan { get; set; }

    internal override void Load(XElement xmlElement)
    {
        Plan = ParseNode<Plan>(xmlElement, nameof(Plan));
    }
}
