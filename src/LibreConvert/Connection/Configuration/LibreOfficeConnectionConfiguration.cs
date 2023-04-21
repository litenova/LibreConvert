namespace LibreConvert.Connection.Configuration;

/// <summary>
/// Represents the configuration settings for the LibreOffice connection.
/// </summary>
public class LibreOfficeConnectionConfiguration
{
    /// <summary>
    /// Gets or sets the path to the LibreOffice executable path. If not set, the default installation path will be used.
    /// </summary>
    public string? ExecutablePath { get; init; } = null;

    /// <summary>
    /// Gets or sets the path to the LibreOffice user profile folder. If not set, a temporary path will be used.
    /// </summary>
    public string? UserInstallation { get; init; } = null;

    /// <summary>
    /// Gets or sets the communication method to use for the LibreOffice connection.
    /// </summary>
    public required LibreOfficeConnectionCommunication Communication { get; init; }
}