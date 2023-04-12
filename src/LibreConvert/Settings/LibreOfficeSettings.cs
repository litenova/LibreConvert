using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace LibreConvert.Settings;

public static class LibreOfficeSettings
{
    private static string? _installPath;

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
        else
        {
            return "";
        }
    }

    public static string InstallPath
    {
        get
        {
            _installPath ??= GetDefaultInstallPath();
            return _installPath;
        }
        set => _installPath = value;
    }

    public static bool Headless { get; set; } = true;

    public static bool Invisible { get; set; } = true;

    public static bool NoLogo { get; set; } = true;

    public static bool NoRestore { get; set; } = true;

    public static bool ConvertToPdf { get; set; } = true;

    public static bool NoLockCheck { get; set; } = true;

    public static bool NoDefault { get; set; } = true;
}