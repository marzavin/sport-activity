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

            var activityNodes = file.Root.Elements().FirstOrDefaultByLocalName(ActivitiesNode)
                ?.Elements().WhereLocalName(ActivityNode).ToList();

            foreach (var activityNode in activityNodes)
            {
                activities.Add(new Activity
                {
                    Id = activityNode.Elements().FirstOrDefaultByLocalName(IdNode)?.Value,
                    Type = activityNode.Attributes().FirstOrDefaultByLocalName(SportAttribute)?.Value,
                    Track = activityNode.Elements().WhereLocalName(LapNode)
                        ?.Select(x => x.Elements().WhereLocalName(TrackNode))
                        ?.SelectMany(x => x.Elements().WhereLocalName(TrackPointNode))
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

        var cadenceNode = trackPointNode.Elements().FirstOrDefaultByLocalName(CadenceNode);
        var altitudeNode = trackPointNode.Elements().FirstOrDefaultByLocalName(AltitudeMetersNode);
        var heartRateValueNode = trackPointNode.Elements().FirstOrDefaultByLocalName(HeartRateNode)
            ?.Elements().FirstOrDefaultByLocalName(ValueNode);

        return new TrackPoint
        {
            TimeStamp = DateTime.Parse(trackPointNode.Elements().FirstByLocalName(TimeNode).Value).ToUniversalTime(),
            Position = GetPosition(trackPointNode.Elements().FirstOrDefaultByLocalName(PositionNode)),
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
            Latitude = double.Parse(positionNode.Elements().FirstByLocalName(LatitudeDegreesNode).Value),
            Longitude = double.Parse(positionNode.Elements().FirstByLocalName(LongitudeDegreesNode).Value)
        };
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
