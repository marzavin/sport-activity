using AM.Sport.Tracking.Models;

namespace AM.Sport.Tracking.Files;

/// <summary>
/// XML based file-container for activities.
/// </summary>
public abstract class XmlActivityContainerBase : IActivityContainer
{
    /// <summary>
    /// Gets or sets path to .tcx file.
    /// </summary>
    protected string Path { get; private set; }

    /// <summary>
    /// Gets or sets source file identifier.
    /// </summary>
    protected string SourceId { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlActivityContainerBase"/> class.
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> in case if <paramref name="path"/> is null or empty string.</exception>
    protected XmlActivityContainerBase(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        Path = path;
        SourceId = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Loads data from .gpx file as <see cref="Activity"/> object.
    /// </summary>
    /// <returns><see cref="Activity"/> for more information.</returns>
    public abstract Task<List<Activity>> LoadAsync();

    /// <summary>
    /// Reads content of XML file as string.
    /// </summary>
    /// <returns></returns>
    protected async Task<string> GetXmlStringAsync()
    {
        string xmlString = null;

        using (var stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
        {
            using (TextReader reader = new StreamReader(stream))
            {
                xmlString = await reader.ReadToEndAsync();
                xmlString = xmlString.Trim();

                reader.Close();
            }

            stream.Close();
        }

        return xmlString;
    }

    /// <summary>
    /// Converts string to UTC DateTime object.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected DateTime ParseUniversalTime(string value)
    {
        return DateTime.Parse(value).ToUniversalTime();
    }
}
