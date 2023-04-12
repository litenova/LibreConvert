using System.Diagnostics;
using LibreConvert.Abstractions;
using LibreConvert.Internal.Utilities;
using LibreConvert.Settings;

namespace LibreConvert.Internal.Process;

internal class LibreOfficeProcess : ILibreOfficeProcess
{
    private readonly System.Diagnostics.Process _libreOfficeProcess;

    /// <summary>
    ///     Returns the full path to LibreOffice, when not found <c>null</c> is returned
    /// </summary>
    internal LibreOfficeProcess()
    {
        var installPath = LibreOfficeSettings.InstallPath;

        _libreOfficeProcess = new System.Diagnostics.Process()
        {
            StartInfo = new ProcessStartInfo(installPath)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
    }

    public async Task<ExecutionResult> ExecuteAsync(Action<ICommandLineArgumentsBuilder> argumentBuilderAction)
    {
        CommandLineArgumentsBuilder argumentBuilder = new();

        if (LibreOfficeSettings.Headless)
        {
            argumentBuilder.Add("--headless");
        }

        if (LibreOfficeSettings.Invisible)
        {
            argumentBuilder.Add("--invisible");
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

        return new ExecutionResult(_libreOfficeProcess.ExitCode, output);
    }

    public void Dispose()
    {
        _libreOfficeProcess.Dispose();
    }
}