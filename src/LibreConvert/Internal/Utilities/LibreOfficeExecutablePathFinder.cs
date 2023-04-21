using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace LibreConvert.Internal.Utilities;

public static class LibreOfficeExecutablePathFinder
{
    public static string Find()
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
            const string linuxInstallPath = "/usr/bin/soffice";

            if (File.Exists(linuxInstallPath))
            {
                return linuxInstallPath;
            }
            else
            {
                throw new InvalidOperationException("Could not find LibreOffice at the default install path");
            }
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            const string macOsInstallPath = "/Applications/LibreOffice.app/Contents/MacOS/soffice";

            if (File.Exists(macOsInstallPath))
            {
                return macOsInstallPath;
            }

            throw new InvalidOperationException("Could not find LibreOffice at the default install path");
        }

        throw new NotSupportedException("The current operating system is not supported");
    }
}