namespace LibreConvert.Connection.Configuration;

/// <summary>
/// Represents communication configuration using TCP for the LibreOffice connection.
/// </summary>
public class LibreOfficeConnectionTcpCommunication : LibreOfficeConnectionCommunication
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LibreOfficeConnectionTcpCommunication"/> class with default settings.
    /// </summary>
    public LibreOfficeConnectionTcpCommunication()
    {
    }

    /// <summary>
    /// Gets or sets the host name or IP address of the LibreOffice connection.
    /// </summary>
    /// <value>The host name or IP address of the connection. Default value is "localhost".</value>
    public string Host { get; init; } = "localhost";

    /// <summary>
    /// Gets or sets the port number of the LibreOffice connection.
    /// </summary>
    /// <value>The port number of the connection. Default value is "2002".</value>
    public int Port { get; init; } = 2002;
}