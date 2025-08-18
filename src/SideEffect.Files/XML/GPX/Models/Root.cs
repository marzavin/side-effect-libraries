using System.Xml.Linq;

namespace SideEffect.Files.XML.GPX.Models;

//TODO:AMZ: Rename this class

/// <summary>
/// Root element of the GPX file.
/// </summary>
internal class Root : XmlFileNode
{
    public Metadata Metadata { get; set; }

    public List<Track> Tracks { get; set; }

    internal override void Load(XElement xmlElement)
    {
        Metadata = ParseNode<Metadata>(xmlElement, "metadata");
        Tracks = ParseNodeList<Track>(xmlElement, "trk");
    }
}
