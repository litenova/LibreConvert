using LibreConvert.Internal.Process;

namespace LibreConvert.Documents;

/// <summary>
/// Represents a Calc document in LibreOffice, which supports the following types of documents:
/// <list type="bullet">
///     <item>ODS (OpenDocument Spreadsheet)</item>
///     <item>XLS (Microsoft Excel 97/2000/XP/2003)</item>
///     <item>XLSX (Microsoft Excel 2007/2010/2013/2016/2019)</item>
///     <item>CSV (Comma-Separated Values)</item>
///     <item>HTML (HyperText Markup Language)</item>
/// </list>
/// </summary>
public class CalcDocument : Document
{
    private readonly string _inputFileFullPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="CalcDocument"/> class with the specified input file.
    /// </summary>
    /// <param name="inputFilePath">The path to the input file.</param>
    public CalcDocument(string inputFilePath)
    {
        _inputFileFullPath = Path.GetFullPath(inputFilePath);
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Exports the Calc document to a PDF file.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task<string> ExportAsPDF()
    {
        var inputFileDirectory = Path.GetDirectoryName(_inputFileFullPath) ?? throw new InvalidOperationException();

        return ExportAsPDF(inputFileDirectory);
    }
    
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Exports the Calc document to a PDF file.
    /// </summary>
    /// <param name="outputDirectory">The path to the output directory.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<string> ExportAsPDF(string outputDirectory)
    {
        const string libreOfficeComponent = "pdf:calc_pdf_Export";

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