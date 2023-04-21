using System.Diagnostics;
using LibreConvert.Abstractions;
using LibreConvert.Connection.Configuration;
using LibreConvert.Internal.Utilities;

namespace LibreConvert.Server;

/// <summary>
/// Represents an instance of LibreOffice running in headless and listening mode.
/// </summary>
public class LibreOfficeServer : ILibreOfficeServer
{
    private readonly Process _libreOfficeProcess;

    /// <summary>
    /// Represents an instance of LibreOffice running in headless and listening mode.
    /// </summary>
    /// <param name="connectionString">The connection string used to connect to LibreOffice.</param>
    /// <param name="executablePath">The path to the LibreOffice executable.</param>
    /// <param name="userProfilePath">The path to the user profile folder for LibreOffice.</param>
    public LibreOfficeServer(string connectionString, string executablePath, string userProfilePath)
    {
        _libreOfficeProcess = new Process
        {
            StartInfo = new ProcessStartInfo(executablePath)
            {
                Arguments = $"--headless --norestore -env:UserInstallation=file:///{userProfilePath.Replace("\\", "/")} --accept='{connectionString}'",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        };

        _libreOfficeProcess.OutputDataReceived += (_, args) => Console.WriteLine(args.Data);
        _libreOfficeProcess.ErrorDataReceived += (_, args) => Console.WriteLine(args.Data);
    }

    /// <summary>
    /// Starts the LibreOffice server.
    /// </summary>
    public void Start()
    {
        _libreOfficeProcess.Start();
        _libreOfficeProcess.BeginOutputReadLine();
        _libreOfficeProcess.BeginErrorReadLine();
    }

    /// <summary>
    /// Stops the LibreOffice server.
    /// </summary>
    public void Stop()
    {
        _libreOfficeProcess.Refresh();

        if (_libreOfficeProcess.HasExited)
        {
            return;
        }

        try
        {
            // Send the Ctrl+C signal to the process to terminate gracefully.
            _libreOfficeProcess.CloseMainWindow();
            _libreOfficeProcess.WaitForExit(TimeSpan.FromSeconds(2));

            if (!_libreOfficeProcess.HasExited)
            {
                // If the process didn't exit gracefully within 2 seconds, kill it.
                _libreOfficeProcess.Kill();
            }
        }
        finally
        {
            _libreOfficeProcess.Dispose();
        }
    }

    /// <summary>
    /// Stops the LibreOffice server and disposes of all resources used by the object.
    /// </summary>
    public void Dispose()
    {
        Stop();
    }
}