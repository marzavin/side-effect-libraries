using NUnit.Framework;
using SideEffect.Files.XML.TCX;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace SideEffect.Files.Tests.XML.TCX;

public class TrainingCenterFileTests
{
    [TestCase("XML\\TCX\\Data\\ride-0-2025-07-26-08-18-41.tcx")]
    public async Task ParseFileTest(string path)
    {
        var validationResult = ValidateDocument("XML\\TCX\\Data\\TrainingCenterDatabasev2.xsd", path);

        var file = new TrainingCenterFile();
        await file.LoadAsync(path);

        Assert.That(file.Content, Is.Not.Null);
    }

    private static bool ValidateDocument(string xsdPath, string xmlPath)
    {
        var schema = new XmlSchemaSet();
        schema.Add("http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2", xsdPath);
        var rd = XmlReader.Create(xmlPath);
        XDocument doc = XDocument.Load(rd);
        try
        {
            doc.Validate(schema, ValidationEventHandler);
        }
        catch
        {
            return false;
        }

        return true;
    }

    static void ValidationEventHandler(object sender, ValidationEventArgs e)
    {
        var type = XmlSeverityType.Warning;
        if (Enum.TryParse("Error", out type))
        {
            if (type == XmlSeverityType.Error)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
