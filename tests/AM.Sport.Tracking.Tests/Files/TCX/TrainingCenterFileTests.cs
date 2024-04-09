using AM.Sport.Tracking.Files.TCX;
using NUnit.Framework;

namespace AM.Sport.Tracking.Tests.Files.TCX;

[TestFixture]
public class TrainingCenterFileTests
{
    [TestCase("Data\\21_Poznan_Marathon.tcx")]
    public async Task LoadFileTest(string path)
    {
        var fullPath = Path.Combine(Environment.CurrentDirectory, path);
        var file = new TrainingCenterFile(fullPath);
        var activities = await file.LoadAsync();

        var track = activities.First().Track;

        var heartRate = TrackCalculator.CalculateAverageHeartRate(track);
        var cadence = TrackCalculator.CalculateAverageCadence(track);
        var minAltitude = TrackCalculator.CalculateMinAltitude(track);
        var maxAltitude = TrackCalculator.CalculateMaxAltitude(track);
    }
}
