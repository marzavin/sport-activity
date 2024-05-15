namespace AM.Sport.Tracking.Models;

/// <summary>
/// Segment(lap) from the source file.
/// </summary>
public class Segment
{
    /// <summary>
    /// Gets or sets number of segment.
    /// </summary>
    public int Number { get; set; }

    public TimeSpan? TotalTime { get; set; }
	
    public double? Distance { get; set; }

    public double? MaxSpeed { get; set; }
}
