using LibreConvert.Abstractions;
using LibreConvert.Client;

namespace LibreConvert.Connection;

/// <summary>
/// Represents a connection to a LibreOffice server and provides access to the client and server objects.
/// </summary>
public interface ILibreOfficeConnection
{
    /// <summary>
    /// Gets the client object associated with the connection.
    /// </summary>
    ILibreOfficeClient Client { get; }

    /// <summary>
    /// Gets the server object associated with the connection.
    /// </summary>
    ILibreOfficeServer Server { get; }
}