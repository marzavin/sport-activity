using AM.Sport.Tracking.Models;

namespace AM.Sport.Tracking.Files.GPX;

/// <summary>
/// Global Positioning XML file.
/// </summary>
public class GlobalPositioningFile : IActivityContainer
{
    /// <summary>
    /// Loads data from .gpx file as <see cref="Activity"/> object.
    /// </summary>
    /// <returns><see cref="Activity"/> for more information.</returns>
    public Task<List<Activity>> LoadAsync()
    {
        throw new NotImplementedException();
    }
}
