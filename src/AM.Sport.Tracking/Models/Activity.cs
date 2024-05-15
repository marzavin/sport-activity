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

    /// <summary>
    /// Gets or sets author of the activity file.
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Gets or sets information about activity segments.
    /// </summary>
    public List<Segment> Segments { get; set; }
}
