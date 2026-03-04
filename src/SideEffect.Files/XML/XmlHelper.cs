using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SideEffect.Files.XML;

/// <summary>
/// A container for helper methods that work with XML content.
/// </summary>
public static class XmlHelper
{
    /// <summary>
    /// Removes unnecessary whitespace from XML content.
    /// </summary>
    /// <param name="input">Input data as a byte array.</param>
    /// <returns>The minified XML content as a byte array.</returns>
    public static byte[] Minify(byte[] input)
    {
        if (input is null || input.Length == 0)
        {
            return [];
        }

        var xmlString = Encoding.UTF8.GetString(input).Trim();

        var document = XDocument.Parse(xmlString, LoadOptions.None);

        using var memoryStream = new MemoryStream();
        using var writer = XmlWriter.Create(memoryStream, new XmlWriterSettings
        {
            Indent = false,
            NewLineHandling = NewLineHandling.None,
            Encoding = new UTF8Encoding(false)
        });

        document.Save(writer);
        writer.Flush();

        return memoryStream.ToArray();
    }
}
