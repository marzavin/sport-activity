using AM.Sport.Tracking.Models;
using System.Xml.Linq;

namespace AM.Sport.Tracking.Files.TCX;

/// <summary>
/// Training Center XML file.
/// </summary>
public partial class TrainingCenterFile : IActivityContainer
{
    /// <summary>
    /// Gets or sets path to .tcx file.
    /// </summary>
    protected string Path { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrainingCenterFile"/> class.
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> in case if <paramref name="path"/> is null or empty string.</exception>
    public TrainingCenterFile(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        Path = path;
    }

    /// <summary>
    /// Loads data from .tcx file as <see cref="Activity"/> object.
    /// </summary>
    /// <returns><see cref="Activity"/> for more information.</returns>
    public async Task<List<Activity>> LoadAsync()
    {
        var activities = new List<Activity>();

        try
        {
            var xmlString = await GetXmlStringAsync();
            var file = XDocument.Parse(xmlString);

            var activityNodes = file.Root.Elements().FirstOrDefault(x => FilterByLocalName(x, ActivitiesNode))
                ?.Elements().Where(x => FilterByLocalName(x, ActivityNode)).ToList();

            foreach (var activityNode in activityNodes)
            {
                activities.Add(new Activity
                {
                    Id = activityNode.Elements().FirstOrDefault(x => FilterByLocalName(x, IdNode))?.Value,
                    Type = activityNode.Attributes().FirstOrDefault(x => FilterByLocalName(x, SportAttribute))?.Value,
                    Track = activityNode.Elements().Where(x => FilterByLocalName(x, LapNode))
                        ?.Select(x => x.Elements().Where(x => FilterByLocalName(x, TrackNode)))
                        ?.SelectMany(x => x.Elements().Where(x => FilterByLocalName(x, TrackPointNode)))
                        ?.Select(GetTrackPoint).OrderBy(x => x.TimeStamp).ToList()
                });
            }
        }
        catch (Exception ex)
        {
            var m = ex.Message;
        }

        return activities;
    }

    private TrackPoint GetTrackPoint(XElement trackPointNode)
    {
        if (trackPointNode == null)
        {
            return null;
        }

        var cadenceNode = trackPointNode.Elements().FirstOrDefault(x => FilterByLocalName(x, CadenceNode));
        var altitudeNode = trackPointNode.Elements().FirstOrDefault(x => FilterByLocalName(x, AltitudeMetersNode));
        var heartRateValueNode = trackPointNode.Elements().FirstOrDefault(x => FilterByLocalName(x, HeartRateNode))
            ?.Elements().FirstOrDefault(x => FilterByLocalName(x, ValueNode));

        return new TrackPoint
        {
            TimeStamp = DateTime.Parse(trackPointNode.Elements().First(x => FilterByLocalName(x, TimeNode)).Value).ToUniversalTime(),
            Position = GetPosition(trackPointNode.Elements().FirstOrDefault(x => FilterByLocalName(x, PositionNode))),
            Cadence = cadenceNode == null ? null : int.Parse(cadenceNode.Value),
            Altitude = altitudeNode == null ? null : double.Parse(altitudeNode.Value),
            HeartRate = heartRateValueNode == null ? null :  int.Parse(heartRateValueNode.Value)
        };
    }

    private Position GetPosition(XElement positionNode)
    {
        if (positionNode == null)
        {
            return null;
        }

        // TODO: Make it culture invariant
        return new Position
        {
            Latitude = double.Parse(positionNode.Elements().First(x => FilterByLocalName(x, LatitudeDegreesNode)).Value),
            Longitude = double.Parse(positionNode.Elements().First(x => FilterByLocalName(x, LongitudeDegreesNode)).Value)
        };
    }

    private bool FilterByLocalName(XElement element, string nodeName)
    {
        return element.Name.LocalName == nodeName;
    }

    private bool FilterByLocalName(XAttribute attribute, string nodeName)
    {
        return attribute.Name.LocalName == nodeName;
    }

    private async Task<string> GetXmlStringAsync()
    {
        string xmlString = null;

        using (var stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
        {
            using (TextReader reader = new StreamReader(stream))
            {
                xmlString = await reader.ReadToEndAsync();
                xmlString = xmlString.Trim();

                reader.Close();
            }

            stream.Close();
        }

        return xmlString;
    }
}
