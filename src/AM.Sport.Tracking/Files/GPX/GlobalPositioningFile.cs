using AM.Sport.Tracking.Models;
using System.Xml.Linq;

namespace AM.Sport.Tracking.Files.GPX;

/// <summary>
/// Global Positioning XML file.
/// </summary>
public partial class GlobalPositioningFile : XmlActivityContainerBase, IActivityContainer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalPositioningFile"/> class.
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> in case if <paramref name="path"/> is null or empty string.</exception>
    public GlobalPositioningFile(string path)
        : base(path)
    { }

    /// <summary>
    /// Loads data from .gpx file as <see cref="Activity"/> object.
    /// </summary>
    /// <returns><see cref="Activity"/> for more information.</returns>
    public override async Task<List<Activity>> LoadAsync()
    {
        var xmlString = await GetXmlStringAsync();
        var file = XDocument.Parse(xmlString);

        var activityNode = file.Root.Elements().FirstByLocalName(TrackNode);

        var activity = new Activity
        {
            SourceId = SourceId,
        };

        return [activity];
    }
}
