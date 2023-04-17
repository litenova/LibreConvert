using LibreConvert.Internal.Process;

namespace LibreConvert.Documents;

/// <summary>
/// Represents a Writer document in LibreOffice, which supports the following types of documents:
/// <list type="bullet">
///     <item>ODT (OpenDocument Text)</item>
///     <item>DOC (Microsoft Word 97/2000/XP/2003)</item>
///     <item>DOCX (Microsoft Word 2007/2010/2013/2016/2019)</item>
///     <item>RTF (Rich Text Format)</item>
///     <item>TXT (Plain Text)</item>
///     <item>HTML (HyperText Markup Language)</item>
/// </list>
/// </summary>
public class WriterDocument : Document
{
    private readonly string _inputFileFullPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="WriterDocument"/> class with the specified input file.
    /// </summary>
    /// <param name="inputFilePath">The path to the input file.</param>
    public WriterDocument(string inputFilePath)
    {
        _inputFileFullPath = Path.GetFullPath(inputFilePath);
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Exports the Writer document to an HTML file.
    /// </summary>
    /// <param name="outputDirectory">The path to the output directory.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ExportAsHTML(string outputDirectory)
    {
        const string libreOfficeComponent = "html:HTML (StarWriter)";

        var libreOfficeProcess = new LibreOfficeProcess();

        await libreOfficeProcess.ExecuteAsync(builder =>
        {
            builder.Add($"--convert-to", libreOfficeComponent, true)
                   .Add("--outdir", outputDirectory, true)
                   .Add("\"" + _inputFileFullPath + "\"");
        });
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Exports the Writer document to a PDF file.
    /// </summary>
    /// <returns>The full path to the created PDF file.</returns>
    public Task<string> ExportAsPDF()
    {
        var inputFileDirectory = Path.GetDirectoryName(_inputFileFullPath) ?? throw new InvalidOperationException();

        return ExportAsPDF(inputFileDirectory);
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Exports the Writer document to a PDF file.
    /// </summary>
    /// <param name="outputDirectory">The path to the output directory.</param>
    /// <returns>The full path to the created PDF file.</returns>
    public async Task<string> ExportAsPDF(string outputDirectory)
    {
        const string libreOfficeComponent = "pdf:writer_pdf_Export";

        var outputDirectoryFullPath = Path.GetFullPath(outputDirectory);

        using var libreOfficeProcess = new LibreOfficeProcess();

        await libreOfficeProcess.ExecuteAsync(builder =>
        {
            builder.Add($"--convert-to", libreOfficeComponent)
                   .Add("--outdir", outputDirectoryFullPath, true)
                   .Add("\"" + _inputFileFullPath + "\"");
        });

        return Path.Combine(outputDirectoryFullPath, Path.GetFileNameWithoutExtension(_inputFileFullPath) + ".pdf");
    }
}