namespace LibreConvert.Abstractions;

/// <summary>
/// Represents an interface for interacting with a LibreOffice process, providing the ability to execute commands asynchronously
/// with the specified command line arguments.
/// </summary>
/// <remarks>
/// This interface also implements the <see cref="IDisposable"/> interface, allowing resources to be properly released
/// after the process is no longer needed.
/// </remarks>
public interface ILibreOfficeProcess : IDisposable
{
    /// <summary>
    /// Executes a command asynchronously with the specified command line arguments.
    /// </summary>
    /// <param name="argumentBuilderAction">An action to be performed on the <see cref="ICommandLineArgumentsBuilder"/> interface
    ///     to build the command line arguments.</param>
    /// <returns>A task representing the asynchronous operation, which returns an <see cref="IExecutionResult"/> object containing
    /// the result of the execution.</returns>
    Task<IExecutionResult> ExecuteAsync(Action<ICommandLineArgumentsBuilder> argumentBuilderAction);
}