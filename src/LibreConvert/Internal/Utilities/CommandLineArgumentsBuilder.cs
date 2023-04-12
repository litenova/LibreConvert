using LibreConvert.Abstractions;

namespace LibreConvert.Internal.Utilities;

internal class CommandLineArgumentsBuilder : ICommandLineArgumentsBuilder
{
    private readonly List<string> _arguments = new();

    public ICommandLineArgumentsBuilder Add(string argument)
    {
        return Add(argument, string.Empty);
    }

    public ICommandLineArgumentsBuilder Add(string argument, string value, bool wrapValueInQuotes = false)
    {
        if (string.IsNullOrEmpty(argument))
        {
            throw new ArgumentException("Argument cannot be null or empty", nameof(argument));
        }

        value = wrapValueInQuotes ? $"\"{value}\"" : value;

        _arguments.Add(string.IsNullOrEmpty(value) ? argument : $"{argument} {value}");

        return this;
    }

    public ICommandLineArgumentsBuilder Remove(string argument)
    {
        _arguments.RemoveAll(a => a == argument || a.StartsWith($"{argument} "));

        return this;
    }

    public ICommandLineArgumentsBuilder Clear()
    {
        _arguments.Clear();

        return this;
    }

    public string Build()
    {
        return string.Join(" ", _arguments);
    }
}