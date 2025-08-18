using System.Xml.Linq;

namespace SideEffect.Files.XML;

/// <summary>
/// Base class for XML file node.
/// </summary>
public abstract class XmlFileNode
{
    internal abstract void Load(XElement xmlElement);

    /// <summary>
    /// Loads single XML node.
    /// </summary>
    /// <param name="xmlElement">See <see cref="XElement"/> for more information.</param>
    /// <param name="localName">Name of the element.</param>
    /// <typeparam name="TNode"></typeparam>
    /// <returns>XML node.</returns>
    protected TNode ParseNode<TNode>(XElement xmlElement, string localName)
        where TNode : XmlFileNode, new()
    {
        var nodeElement = xmlElement.Elements().FirstOrDefaultByLocalName(localName);
        if (nodeElement is null)
        {
            return null;
        }

        var node = new TNode();

        node.Load(nodeElement);

        return node;
    }

    /// <summary>
    /// Loads list of XML nodes.
    /// </summary>
    /// <param name="xmlElement">See <see cref="XElement"/> for more information.</param>
    /// <param name="localName">Name of the element.</param>
    /// <typeparam name="TNode"></typeparam>
    /// <returns>List of XML nodes.</returns>
    protected List<TNode> ParseNodeList<TNode>(XElement xmlElement, string localName)
        where TNode : XmlFileNode, new()
    {
        var nodeElements = xmlElement.Elements().WhereLocalName(localName);
        if (nodeElements is null)
        {
            return null;
        }

        var nodes = new List<TNode>();

        foreach (var nodeElement in nodeElements)
        {
            var node = new TNode();

            node.Load(nodeElement);

            nodes.Add(node);
        }

        return nodes;
    }

    /// <summary>
    /// Returns value of the child XML node.
    /// </summary>
    /// <param name="xmlElement">See <see cref="XElement"/> for more information.</param>
    /// <param name="localName">Name of the element.</param>
    /// <param name="childLocalName">Name of the child element.</param>
    /// <returns></returns>
    protected string ParseNodeChildValue(XElement xmlElement, string localName, string childLocalName)
    {
        var nodeElement = xmlElement.Elements().FirstOrDefaultByLocalName(localName);
        if (nodeElement is null)
        {
            return null;
        }

        var childNode = nodeElement.Elements().FirstByLocalName(childLocalName);
        if (childNode is null)
        {
            return null;
        }

        return childNode.Value;
    }

    /// <summary>
    /// Loads child XML node.
    /// </summary>
    /// <param name="xmlElement">See <see cref="XElement"/> for more information.</param>
    /// <param name="localName">Name of the element.</param>
    /// <param name="childLocalName">Name of the child element.</param>
    /// <typeparam name="TNode"></typeparam>
    /// <returns></returns>
    protected List<TNode> ParseNodeChildren<TNode>(XElement xmlElement, string localName, string childLocalName)
        where TNode : XmlFileNode, new()
    {
        var nodeElement = xmlElement.Elements().FirstOrDefaultByLocalName(localName);
        if (nodeElement is null)
        {
            return null;
        }

        var childrenNodes = nodeElement.Elements().WhereLocalName(childLocalName);
        if (childrenNodes is null)
        {
            return null;
        }

        var nodes = new List<TNode>();

        foreach (var childElement in childrenNodes)
        {
            var node = new TNode();

            node.Load(childElement);

            nodes.Add(node);
        }

        return nodes;
    }

    /// <summary>
    /// Converts string to UTC DateTime object.
    /// </summary>
    /// <param name="value">String value.</param>
    /// <returns>Parsed value.</returns>
    protected DateTime ParseUniversalTime(string value)
    {
        return DateTime.Parse(value).ToUniversalTime();
    }

    //TODO:AMZ: Add IFormatProvider

    /// <summary>
    /// Converts optional string value to nullable double.
    /// </summary>
    /// <param name="value">String value.</param>
    /// <returns>Parsed value.</returns>
    protected double? ParseNullableDouble(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : double.Parse(value);
    }

    /// <summary>
    /// Converts string value to double.
    /// </summary>
    /// <param name="value">String value.</param>
    /// <returns>Parsed value.</returns>
    protected double ParseDouble(string value)
    {
        return double.Parse(value);
    }

    /// <summary>
    /// Converts optional string value to nullable int.
    /// </summary>
    /// <param name="value">String value.</param>
    /// <returns>Parsed value.</returns>
    protected int? ParseNullableInteger(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : int.Parse(value);
    }

    /// <summary>
    /// Converts string value to int.
    /// </summary>
    /// <param name="value">String value.</param>
    /// <returns>Parsed value.</returns>
    protected int ParseInteger(string value)
    {
        return int.Parse(value);
    }

    /// <summary>
    /// Converts string value to bool.
    /// </summary>
    /// <param name="value">String value.</param>
    /// <returns>Parsed value.</returns>
    protected bool ParseBoolean(string value)
    {
        return bool.Parse(value);
    }
}
