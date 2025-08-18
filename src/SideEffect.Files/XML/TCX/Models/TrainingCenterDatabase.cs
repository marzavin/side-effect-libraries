using System.Xml.Linq;

namespace SideEffect.Files.XML.TCX.Models;

/// <summary>
/// Root element of the TCX file.
/// </summary>
public class TrainingCenterDatabase : XmlFileNode
{
    /// <summary>
    /// Gets or sets list of activities.
    /// </summary>
    public List<Activity> Activities { get; set; }

    /// <summary>
    /// Gets or sets information about file author.
    /// </summary>
    public Author Author { get; set; }

    internal override void Load(XElement xmlElement)
    {
        Activities = ParseNodeChildren<Activity>(xmlElement, nameof(Activities), nameof(Activity));
        Author = ParseNode<Author>(xmlElement, nameof(Author));
    }
}
