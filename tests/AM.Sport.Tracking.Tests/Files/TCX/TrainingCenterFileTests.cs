using AM.Sport.Tracking.Files.TCX;
using NUnit.Framework;

namespace AM.Sport.Tracking.Tests.Files.TCX;

[TestFixture]
public class TrainingCenterFileTests
{
    [TestCase("Data\\strava_21_Poznan_Marathon.tcx")]
    public async Task LoadFileTest(string path)
    {
        var fullPath = Path.Combine(Environment.CurrentDirectory, path);
        var file = new TrainingCenterFile(fullPath);
        var activities = await file.LoadAsync();

        var activity = activities.First();
        var track = activity.Track;

        var heartRate = TrackCalculator.CalculateAverageHeartRate(track);
        var cadence = TrackCalculator.CalculateAverageCadence(track);
        var minAltitude = TrackCalculator.CalculateMinAltitude(track);
        var maxAltitude = TrackCalculator.CalculateMaxAltitude(track);

        var distance = TrackCalculator.CalculateDistanceBetweenCoordinates(track[10].Position, track[11].Position);

        var trackDistance = TrackCalculator.CalculateTrackDistance(track);

        Assert.That(activity.TimeStamp.Kind, Is.EqualTo(DateTimeKind.Utc));

        Assert.That(activity.Segments.Count, Is.EqualTo(9));
        foreach (var segment in activity.Segments)
        {
            Assert.That(segment.StartTime.Kind, Is.EqualTo(DateTimeKind.Utc));
        }
    
        Assert.That(activity.Author, Is.Not.Empty);
    }
}
