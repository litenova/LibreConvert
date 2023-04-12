using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace LibreConvert.Settings;

/// <summary>
/// Provides settings for the LibreOffice process used to convert Office documents to various formats.
/// </summary>
public static class LibreOfficeSettings
{
    private static string? _installPath;

    /// <summary>
    /// Gets or sets the path to the LibreOffice executable file.
    /// </summary>
    public static string InstallPath
    {
        get
        {
            _installPath ??= GetDefaultInstallPath();
            return _installPath;
        }
        set => _installPath = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the LibreOffice process should run in headless mode.
    /// </summary>
    public static bool Headless { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the LibreOffice process should be invisible.
    /// </summary>
    public static bool Invisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the LibreOffice process should display the logo during startup.
    /// </summary>
    public static bool NoLogo { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the LibreOffice process should not restore the last saved view of a document.
    /// </summary>
    public static bool NoRestore { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the LibreOffice process should not check for file locks.
    /// </summary>
    public static bool NoLockCheck { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the LibreOffice process should not use the default template when creating new documents.
    /// </summary>
    public static bool NoDefault { get; set; } = true;

    /// <summary>
    /// Returns the default install path for LibreOffice on the current operating system.
    /// </summary>
    /// <returns>The default install path for LibreOffice.</returns>
    private static string GetDefaultInstallPath()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var libreOfficeInstallPathRegistryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\LibreOffice\UNO\InstallPath");

            if (libreOfficeInstallPathRegistryKey == null)
            {
                throw new InvalidOperationException("Could not find LibreOffice install path in the registry");
            }

            if (libreOfficeInstallPathRegistryKey.GetValue(string.Empty) is not string installPath)
            {
                throw new InvalidOperationException("Could not find LibreOffice install path in the registry");
            }

            return Path.Combine(installPath, "soffice.exe");
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "/usr/bin/soffice";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "/Applications/LibreOffice.app/Contents/MacOS/soffice";
        }

        throw new NotSupportedException("The current operating system is not supported");
    }
}