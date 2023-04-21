namespace LibreConvert.Connection.Configuration;

/// <summary>
/// Represents a communication channel using a named pipe for a LibreOffice connection.
/// </summary>
public class LibreOfficeConnectionPipeCommunication : LibreOfficeConnectionCommunication
{
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LibreOfficeConnectionPipeCommunication"/> class with guid based name.
    /// </summary>
    public LibreOfficeConnectionPipeCommunication()
    {
    }
    
    /// <summary>
    /// Gets or sets the name of the named pipe to use for communication.
    /// </summary>
    public string Name { get; init; } = Guid.NewGuid().ToString("N");
}