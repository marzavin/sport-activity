﻿using AM.Sport.Tracking.Models;
using System.Xml.Linq;

namespace AM.Sport.Tracking.Files.TCX;

/// <summary>
/// Training Center XML file.
/// </summary>
public partial class TrainingCenterFile : XmlActivityContainerBase, IActivityContainer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TrainingCenterFile"/> class.
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> in case if <paramref name="path"/> is null or empty string.</exception>
    public TrainingCenterFile(string path)
        : base(path)
    { }

    /// <summary>
    /// Loads data from .tcx file as <see cref="Activity"/> object.
    /// </summary>
    /// <returns><see cref="Activity"/> for more information.</returns>
    public override async Task<List<Activity>> LoadAsync()
    {
        var sourceId = Guid.NewGuid();

        var activities = new List<Activity>();

        var xmlString = await GetXmlStringAsync();
        var file = XDocument.Parse(xmlString);

        var activityNodes = file.Root.Elements().FirstOrDefaultByLocalName(ActivitiesNode)
            ?.Elements().WhereLocalName(ActivityNode).ToList();

        var author = file.Root
            .Elements().FirstOrDefaultByLocalName(AuthorNode)?
            .Elements().FirstOrDefaultByLocalName(NameNode).Value;

        foreach (var activityNode in activityNodes)
        {
            var activity = new Activity
            {
                SourceId = sourceId,
                TimeStamp = ParseUniversalTime(activityNode.Elements().FirstOrDefaultByLocalName(IdNode)?.Value),
                Author = author,
                Type = activityNode.Attributes().FirstOrDefaultByLocalName(SportAttribute)?.Value
            };

            var laps = activityNode.Elements().WhereLocalName(LapNode).ToList();
            var track = new List<TrackPoint>();

            for (var i = 0; i < laps.Count; i++)
            {
                var startTime = laps[i].Attributes().FirstByLocalName(StartTimeAttribute)?.Value;
                var distanceValue = laps[i].Elements().FirstByLocalName(DistanceMetersNode).Value;
                var totalTimeValue = laps[i].Elements().FirstByLocalName(TotalTimeSecondsNode).Value;
                var maxSpeedValue = laps[i].Elements().FirstOrDefaultByLocalName(MaximumSpeedNode)?.Value;

                var segment = new Segment
                {
                    Number = i + 1,
                    StartTime = ParseUniversalTime(startTime),
                    Distance = double.Parse(distanceValue),
                    TotalTime = TimeSpan.FromSeconds(double.Parse(totalTimeValue)),
                    MaxSpeed = string.IsNullOrWhiteSpace(maxSpeedValue) ? null : double.Parse(maxSpeedValue)
                };

                activity.Segments ??= [];
                activity.Segments.Add(segment);

                var newPoints = laps[i].Elements()
                    ?.FirstOrDefaultByLocalName(TrackNode)
                    ?.Elements().WhereLocalName(TrackPointNode)
                    ?.Select(x => GetTrackPoint(x, segment.Number)).OrderBy(x => x.TimeStamp).ToList();

                if (newPoints != null && newPoints.Count > 0)
                {
                    track.AddRange(newPoints);
                }
            }

            activity.Track = track.Count > 0 ? track : null;

            activities.Add(activity);
        }

        return activities;
    }

    private TrackPoint GetTrackPoint(XElement trackPointNode, int? segmentNumber = null)
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
            HeartRate = heartRateValueNode == null ? null : int.Parse(heartRateValueNode.Value),
            SegmentNumber = segmentNumber
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
}
