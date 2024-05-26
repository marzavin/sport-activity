namespace AM.Sport.Tracking.Models;

/// <summary>
/// Segment(lap) from the source file.
/// </summary>
public class Segment
{
    /// <summary>
    /// Gets or sets number of the segment.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Gets or sets start time of the segment.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets total time of the segment.
    /// </summary>
    public TimeSpan TotalTime { get; set; }

    /// <summary>
    /// Gets or sets distance of the segment.
    /// </summary>
    public double Distance { get; set; }

    /// <summary>
    /// Gets or sets maximum speed shown on the segment.
    /// </summary>
    public double? MaxSpeed { get; set; }
}
