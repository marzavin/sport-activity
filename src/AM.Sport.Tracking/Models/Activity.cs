namespace AM.Sport.Tracking.Models;

/// <summary>
/// Base information about sport activity.
/// </summary>
public class Activity
{
    /// <summary>
    /// Gets or sets activity id;
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets activity type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets list of <see cref="TrackPoint"/> of the activity track.
    /// </summary>
    public List<TrackPoint> Track { get; set; }
}
