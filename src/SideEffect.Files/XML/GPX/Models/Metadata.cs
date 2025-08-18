using System.Xml.Linq;

namespace SideEffect.Files.XML.GPX.Models;

internal class Metadata : XmlFileNode
{
    public DateTime Time { get; set; }

    internal override void Load(XElement xmlElement)
    {
        //TODO:AMZ: Check if this field is required
        Time = ParseUniversalTime(xmlElement.GetRequiredElementValue("time"));
    }
}
