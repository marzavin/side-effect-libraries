using SideEffect.Files.XML.Exceptions;
using System.Xml.Linq;

namespace SideEffect.Files.XML;

/// <summary>
/// Extension methods for Linq to XML classes
/// </summary>
public static class XmlExtensions
{
    #region Attributes

    /// <summary>
    /// Returns value of the required attribute as string.
    /// </summary>
    /// <param name="xmlElement">See <see cref="XElement"/> for more information.</param>
    /// <param name="localName">Name of the attribute.</param>
    /// <returns>Value of the attribute.</returns>
    public static string GetRequiredAttributeValue(this XElement xmlElement, string localName)
    {
        return xmlElement.GetAttributeValue(localName, true);
    }

    /// <summary>
    /// Returns value of the optional attribute as string.
    /// </summary>
    /// <param name="xmlElement">See <see cref="XElement"/> for more information.</param>
    /// <param name="localName">Name of the attribute.</param>
    /// <returns>Value of the attribute.</returns>
    public static string GetOptionalAttributeValue(this XElement xmlElement, string localName)
    {
        return xmlElement.GetAttributeValue(localName, false);
    }

    /// <summary>
    /// Returns value of the attribute as string.
    /// </summary>
    /// <param name="xmlElement">See <see cref="XElement"/> for more information.</param>
    /// <param name="localName">Name of the attribute.</param>
    /// <param name="isRequired">Flag for required attribute.</param>
    /// <returns>Value of the attribute.</returns>
    private static string GetAttributeValue(this XElement xmlElement, string localName, bool isRequired)
    {
        if (xmlElement is null)
        {
            throw new ArgumentNullException(nameof(xmlElement));
        }

        if (string.IsNullOrWhiteSpace(localName))
        {
            throw new ArgumentNullException(nameof(localName));
        }

        var attribute = xmlElement.Attributes().FirstOrDefault(x => FilterByLocalName(x, localName));
        if (attribute is null && isRequired)
        {
            throw new MissingItemException($"Required attribute '{localName}' is missing.");
        }

        return attribute?.Value;
    }

    #endregion

    #region Elements
    
    /// <summary>
    /// Returns value of the required element as string.
    /// </summary>
    /// <param name="xmlElement">See <see cref="XElement"/> for more information.</param>
    /// <param name="localName">Name of the element.</param>
    /// <returns>Value of the attribute.</returns>
    public static string GetRequiredElementValue(this XElement xmlElement, string localName)
    {
        return xmlElement.GetElementValue(localName, true);
    }

    /// <summary>
    /// Returns value of the optional element as string.
    /// </summary>
    /// <param name="xmlElement">See <see cref="XElement"/> for more information.</param>
    /// <param name="localName">Name of the element.</param>
    /// <returns>Value of the attribute.</returns>
    public static string GetOptionalElementValue(this XElement xmlElement, string localName)
    {
        return xmlElement.GetElementValue(localName, false);
    }
    
    private static string GetElementValue(this XElement xmlElement, string localName, bool isRequired)
    {
        if (xmlElement is null)
        {
            throw new ArgumentNullException(nameof(xmlElement));
        }

        if (string.IsNullOrWhiteSpace(localName))
        {
            throw new ArgumentNullException(nameof(localName));
        }
        
        var element = xmlElement.Elements().FirstOrDefault(x => FilterByLocalName(x, localName));
        if (element is null && isRequired)
        {
            throw new MissingItemException($"Required element '{localName}' is missing.");
        }

        return element?.Value;
    }
    
    #endregion

    /// <summary>
    /// Applies filter by local name to <see cref="IEnumerable{XElement}"/>.
    /// </summary>
    /// <param name="elements">Source <see cref="IEnumerable{XElement}"/>.</param>
    /// <param name="name">Local name of <see cref="XElement"/>.</param>
    /// <returns><see cref="IEnumerable{XElement}"/> filtered by local name.</returns>
    public static IEnumerable<XElement> WhereLocalName(this IEnumerable<XElement> elements, string name)
    {
        return elements.Where(x => FilterByLocalName(x, name));
    }

    /// <summary>
    /// Searches for the first <see cref="XElement"/> with specified local name.
    /// </summary>
    /// <param name="elements">Source <see cref="IEnumerable{XElement}"/>.</param>
    /// <param name="name">Local name of <see cref="XElement"/>.</param>
    /// <returns>First found <see cref="XElement"/> or default value (null).</returns>
    public static XElement FirstOrDefaultByLocalName(this IEnumerable<XElement> elements, string name)
    {
        return elements.FirstOrDefault(x => FilterByLocalName(x, name));
    }

    /// <summary>
    /// Searches for the first <see cref="XElement"/> with specified local name.
    /// </summary>
    /// <param name="elements">Source <see cref="IEnumerable{XElement}"/>.</param>
    /// <param name="name">Local name of <see cref="XElement"/>.</param>
    /// <returns>First found <see cref="XElement"/>.</returns>
    public static XElement FirstByLocalName(this IEnumerable<XElement> elements, string name)
    {
        return elements.First(x => FilterByLocalName(x, name));
    }

    /// <summary>
    /// Searches for the first <see cref="XAttribute"/> with specified local name.
    /// </summary>
    /// <param name="attributes">Source <see cref="IEnumerable{XAttribute}"/>.</param>
    /// <param name="name">Local name of <see cref="XAttribute"/>.</param>
    /// <returns>First found <see cref="XAttribute"/> or default value (null).</returns>
    public static XAttribute FirstOrDefaultByLocalName(this IEnumerable<XAttribute> attributes, string name)
    {
        return attributes.FirstOrDefault(x => FilterByLocalName(x, name));
    }

    /// <summary>
    /// Searches for the first <see cref="XAttribute"/> with specified local name.
    /// </summary>
    /// <param name="attributes">Source <see cref="IEnumerable{XAttribute}"/>.</param>
    /// <param name="name">Local name of <see cref="XAttribute"/>.</param>
    /// <returns>First found <see cref="XAttribute"/>.</returns>
    public static XAttribute FirstByLocalName(this IEnumerable<XAttribute> attributes, string name)
    {
        return attributes.First(x => FilterByLocalName(x, name));
    }

    private static bool FilterByLocalName(XElement element, string name)
    {
        return string.Equals(element.Name.LocalName, name, StringComparison.InvariantCultureIgnoreCase);
    }

    private static bool FilterByLocalName(XAttribute attribute, string name)
    {
        return string.Equals(attribute.Name.LocalName, name, StringComparison.InvariantCultureIgnoreCase);
    }
}
