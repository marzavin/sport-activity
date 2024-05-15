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

    private static double ConvertDegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180D;
    }

    /// <summary>
    /// Calculates distance between two GPS points.
    /// </summary>
    /// <param name="startPoint">Start point.</param>
    /// <param name="endPoint">End point.</param>
    /// <returns>Distance between two GPS points in metres.</returns>
    public static double CalculateDistanceBetweenCoordinates(Position startPoint, Position endPoint)
    {
        var earthRadiusKm = 6378.1370D;

        var distanceLatitude = ConvertDegreesToRadians(endPoint.Latitude - startPoint.Latitude);
        var distanceLongitude = ConvertDegreesToRadians(endPoint.Longitude - startPoint.Longitude);

        var startLatitudeRadians = ConvertDegreesToRadians(startPoint.Latitude);
        var endLatitudeRadians = ConvertDegreesToRadians(endPoint.Latitude);

        var a = Math.Sin(distanceLatitude / 2) * Math.Sin(distanceLatitude / 2) +
                Math.Sin(distanceLongitude / 2) * Math.Sin(distanceLongitude / 2) * Math.Cos(startLatitudeRadians) * Math.Cos(endLatitudeRadians);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return earthRadiusKm * c * 1000D;
    }

    /// <summary>
    /// Calculates full distance of the track.
    /// </summary>
    /// <param name="track">List of <see cref="TrackPoint"/>.</param>
    /// <returns>Full distance of the track.</returns>
    /// <exception cref="ArgumentNullException">Track parameter is 'null'.</exception>
    public static double CalculateTrackDistance(List<TrackPoint> track)
    {
        if (track == null)
        {
            throw new ArgumentNullException(nameof(track));
        }

        var distance = 0D;

        var points = track.Where(x => x.Position is not null).OrderBy(x => x.TimeStamp).ToList();

        if (points.Count < 2)
        {
            return distance;
        }      

        for (var i = 1; i < points.Count; i++)
        {
            distance += CalculateDistanceBetweenCoordinates(points[i - 1].Position, points[i].Position);
        }

        return distance;
    }
}
