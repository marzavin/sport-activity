using System.Xml.Linq;

namespace AM.Sport.Tracking.Files;

/// <summary>
/// Extension methods for Linq to XML classes
/// </summary>
internal static class XmlExtensions
{
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

    private static bool FilterByLocalName(XElement element, string name)
    {
        return string.Equals(element.Name.LocalName, name, StringComparison.InvariantCultureIgnoreCase);
    }

    private static bool FilterByLocalName(XAttribute attribute, string name)
    {
        return string.Equals(attribute.Name.LocalName, name, StringComparison.InvariantCultureIgnoreCase);
    }
}
