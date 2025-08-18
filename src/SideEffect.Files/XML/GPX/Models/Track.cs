using System.Xml.Linq;

namespace SideEffect.Files.XML.GPX.Models;

internal class Track : XmlFileNode
{
    public string Name { get; set; }

    public string Type { get; set; }

    public List<Segment> Segments { get; set; }

    internal override void Load(XElement xmlElement)
    {
        //TODO:AMZ: Check if this field is required
        Name = xmlElement.GetOptionalElementValue("name");
        Type = xmlElement.GetOptionalElementValue("type");
        Segments = ParseNodeList<Segment>(xmlElement, "trkseg");
    }
}
