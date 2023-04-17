using LibreConvert.Internal.Process;

namespace LibreConvert.Documents;

/// <summary>
/// Represents an Impress document in LibreOffice, which supports the following types of documents:
/// <list type="bullet">
///     <item>ODP (OpenDocument Presentation)</item>
///     <item>PPT (Microsoft PowerPoint 97/2000/XP/2003)</item>
///     <item>PPTX (Microsoft PowerPoint 2007/2010/2013/2016/2019)</item>
/// </list>
/// </summary>
public class ImpressDocument : Document
{
    private readonly string _inputFileFullPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImpressDocument"/> class with the specified input file.
    /// </summary>
    /// <param name="inputFilePath">The path to the input file.</param>
    public ImpressDocument(string inputFilePath)
    {
        _inputFileFullPath = Path.GetFullPath(inputFilePath);
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Exports the Impress document to a PDF file.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task<string> ExportAsPDF()
    {
        var inputFileDirectory = Path.GetDirectoryName(_inputFileFullPath) ?? throw new InvalidOperationException();

        return ExportAsPDF(inputFileDirectory);
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Exports the Impress document to a PDF file.
    /// </summary>
    /// <param name="outputDirectory">The path to the output directory.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<string> ExportAsPDF(string outputDirectory)
    {
        const string libreOfficeComponent = "pdf:impress_pdf_Export";

        var outputDirectoryFullPath = Path.GetFullPath(outputDirectory);

        using var libreOfficeProcess = new LibreOfficeProcess();

        await libreOfficeProcess.ExecuteAsync(builder =>
        {
            builder.Add($"--convert-to", libreOfficeComponent)
                   .Add("--outdir", outputDirectory, true)
                   .Add("\"" + _inputFileFullPath + "\"");
        });

        return Path.Combine(outputDirectoryFullPath, Path.GetFileNameWithoutExtension(_inputFileFullPath) + ".pdf");
    }
}