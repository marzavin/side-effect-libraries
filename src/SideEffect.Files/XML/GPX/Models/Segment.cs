using System.Xml.Linq;

namespace SideEffect.Files.XML.GPX.Models;

internal class Segment : XmlFileNode
{
    public List<TrackPoint> Trackpoints { get; set; }

    internal override void Load(XElement xmlElement)
    {
        Trackpoints = ParseNodeList<TrackPoint>(xmlElement, "trkpt");
    }
}
