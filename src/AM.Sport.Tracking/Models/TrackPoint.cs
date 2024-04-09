namespace AM.Sport.Tracking.Models;

/// <summary>
/// Information about single point of the track.
/// </summary>
public class TrackPoint
{
    /// <summary>
    /// Gets or sets point date and time.
    /// </summary>
    public DateTime TimeStamp { get; set; }

    /// <summary>
    /// Gets or sets point GPS position.
    /// </summary>
    public Position Position { get; set; }

    /// <summary>
    /// Gets or sets altitude at the moment of time.
    /// </summary>
    public double? Altitude { get; set; }

    /// <summary>
    /// Gets or sets cadence at the moment of time.
    /// </summary>
    public int? Cadence { get; set; }

    /// <summary>
    /// Gets or sets heart rate at the moment of time. 
    /// </summary>
    public int? HeartRate { get; set; }
}
