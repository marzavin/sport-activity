namespace AM.Sport.Tracking.Models;

/// <summary>
/// Base information about sport activity.
/// </summary>
public class Activity
{
    /// <summary>
    /// Gets or sets source identifier.
    /// </summary>
    public Guid SourceId { get; set; }

    /// <summary>
    /// Gets or sets timestamp of activity.
    /// </summary>
    public DateTime TimeStamp { get; set; }

    /// <summary>
    /// Gets or sets activity type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets activity name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets information about activity trackpoints.
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
