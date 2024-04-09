using AM.Sport.Tracking.Models;

namespace AM.Sport.Tracking;

/// <summary>
/// Calculator for track statistics.
/// </summary>
public static class TrackCalculator
{
    /// <summary>
    /// Calculates average heart rate.
    /// </summary>
    /// <param name="track">List of <see cref="TrackPoint"/>.</param>
    /// <returns>Average heart rate.</returns>
    public static int? CalculateAverageHeartRate(List<TrackPoint> track)
    {
        var points = track?.Where(x => x.HeartRate.HasValue).ToList();
        if (points == null || points.Count == 0)
        {
            return null;
        }

        return points.Sum(x => x.HeartRate) / points.Count;
    }

    /// <summary>
    /// Calculates average cadence.
    /// </summary>
    /// <param name="track">List of <see cref="TrackPoint"/>.</param>
    /// <returns>Average cadence.</returns>
    public static int? CalculateAverageCadence(List<TrackPoint> track)
    {
        var points = track?.Where(x => x.Cadence.HasValue).ToList();
        if (points == null || points.Count == 0)
        {
            return null;
        }

        return points.Sum(x => x.Cadence) / points.Count;
    }

    /// <summary>
    /// Calculates max altitude.
    /// </summary>
    /// <param name="track">List of <see cref="TrackPoint"/>.</param>
    /// <returns>Max altitude of the track.</returns>
    public static double? CalculateMaxAltitude(List<TrackPoint> track)
    {
        var points = track?.Where(x => x.Altitude.HasValue).ToList();
        if (points == null || points.Count == 0)
        {
            return null;
        }

        return points.Max(x => x.Altitude);
    }

    /// <summary>
    /// Calculates min altitude.
    /// </summary>
    /// <param name="track">List of <see cref="TrackPoint"/>.</param>
    /// <returns>Max altitude of the track.</returns>
    public static double? CalculateMinAltitude(List<TrackPoint> track)
    {
        var points = track?.Where(x => x.Altitude.HasValue).ToList();
        if (points == null || points.Count == 0)
        {
            return null;
        }

        return points.Min(x => x.Altitude);
    }
}
