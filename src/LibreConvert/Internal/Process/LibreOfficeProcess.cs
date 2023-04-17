using System.Diagnostics;
using LibreConvert.Abstractions;
using LibreConvert.Internal.Utilities;
using LibreConvert.Settings;

namespace LibreConvert.Internal.Process;

/// <summary>
/// Represents a LibreOffice process used for executing document conversions.
/// </summary>
internal class LibreOfficeProcess : ILibreOfficeProcess
{
    private readonly System.Diagnostics.Process _libreOfficeProcess;

    /// <summary>
    /// Initializes a new instance of the <see cref="LibreOfficeProcess"/> class with default settings.
    /// </summary>
    internal LibreOfficeProcess()
    {
        var installPath = LibreOfficeSettings.InstallPath;

        _libreOfficeProcess = new System.Diagnostics.Process()
        {
            StartInfo = new ProcessStartInfo(installPath)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        };
    }

    /// <inheritdoc/>
    public async Task<IExecutionResult> ExecuteAsync(Action<ICommandLineArgumentsBuilder> argumentBuilderAction)
    {
        CommandLineArgumentsBuilder argumentBuilder = new();

        if (LibreOfficeSettings.Headless)
        {
            argumentBuilder.Add("--headless");
        }

        if (LibreOfficeSettings.NoLogo)
        {
            argumentBuilder.Add("--nologo");
        }

        if (LibreOfficeSettings.NoRestore)
        {
            argumentBuilder.Add("--norestore");
        }

        if (LibreOfficeSettings.NoLockCheck)
        {
            argumentBuilder.Add("--nolockcheck");
        }

        if (LibreOfficeSettings.NoDefault)
        {
            argumentBuilder.Add("--nodefault");
        }

        argumentBuilderAction(argumentBuilder);

        _libreOfficeProcess.StartInfo.Arguments = argumentBuilder.Build();

        if (!_libreOfficeProcess.Start())
        {
            throw new InvalidProgramException("Could not start LibreOffice");
        }

        var output = await _libreOfficeProcess.StandardOutput.ReadToEndAsync();

        await _libreOfficeProcess.WaitForExitAsync();

        var exitCode = _libreOfficeProcess.ExitCode;

        // Set the error message if the process returned an error
        string? errorMessage = null;
        if (exitCode != 0)
        {
            errorMessage = await _libreOfficeProcess.StandardError.ReadToEndAsync();
        }

        // Create a new ExecutionResult object with the exit code, output, and error message (if any)
        var executionResult = new ExecutionResult(exitCode, output, _libreOfficeProcess.StartInfo.Arguments, errorMessage);

        return executionResult;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _libreOfficeProcess.Dispose();
    }
}