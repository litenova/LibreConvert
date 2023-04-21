using LibreConvert.Abstractions;
using LibreConvert.Client;
using LibreConvert.Connection.Configuration;
using LibreConvert.Internal.Utilities;
using LibreConvert.Server;

namespace LibreConvert.Connection;

/// <inheritdoc/>
public class LibreOfficeConnection : ILibreOfficeConnection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LibreOfficeConnection"/> class with the specified options.
    /// </summary>
    /// <param name="configuration">The configuration options used to establish the connection.</param>
    public LibreOfficeConnection(LibreOfficeConnectionConfiguration configuration)
    {
        var connectionString = configuration.Communication switch
        {
            LibreOfficeConnectionTcpCommunication tcpCommunication => $"socket,host={tcpCommunication.Host},port={tcpCommunication.Port};urp;StarOffice.ComponentContext",
            LibreOfficeConnectionPipeCommunication pipeCommunication => $"pipe,name={pipeCommunication.Name};urp;StarOffice.ComponentContext",
            _ => throw new ArgumentException("The communication type is not supported.", nameof(configuration))
        };

        var executablePath = Path.GetFullPath( configuration.ExecutablePath ?? LibreOfficeExecutablePathFinder.Find());
        var installPath = Path.GetDirectoryName(executablePath)!.Replace('\\', '/');

        Environment.SetEnvironmentVariable("URE_BOOTSTRAP", $"vnd.sun.star.pathname:{installPath}/fundamental.ini", EnvironmentVariableTarget.Process);

        var environmentPath = Environment.GetEnvironmentVariable("PATH");
        Environment.SetEnvironmentVariable("UNO_PATH", installPath, EnvironmentVariableTarget.Process);

        if (environmentPath != null && !environmentPath.Contains(installPath))
        {
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + @";" + installPath, EnvironmentVariableTarget.Process);
        }

        var temporaryUserInstallationPath = configuration.UserInstallation ?? Path.Combine(Path.GetTempPath(), "LibreOffice", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryUserInstallationPath);

        Client = new LibreOfficeClient(connectionString, executablePath);

        Server = new LibreOfficeServer(connectionString, executablePath, temporaryUserInstallationPath);

        Server.Start();

    }

    /// <inheritdoc/>
    public ILibreOfficeClient Client { get; }

    /// <inheritdoc/>
    public ILibreOfficeServer Server { get; }
}