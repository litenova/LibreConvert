namespace LibreConvert.WebApi;

public static class RunningOperatingSystem
{
    public static string Name => (Environment.GetEnvironmentVariable("RUNNING_OS") ?? "Unknown OS").Trim();
}