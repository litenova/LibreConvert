using LibreConvert.Internal.Process;

namespace LibreConvert.Documents;

public class WordDocument : Document
{
    private readonly string _inputFile;

    public WordDocument(string inputFile)
    {
        _inputFile = inputFile;
    }

    public override async Task ExportAsync(string outputDirectory)
    {
        const string libreOfficeComponent = "pdf:writer_pdf_Export";

        var libreOfficeProcess = new LibreOfficeProcess();

        await libreOfficeProcess.ExecuteAsync(builder =>
        {
            builder.Add($"--convert-to", libreOfficeComponent)
                   .Add("--outdir", "\"" + outputDirectory + "\"")
                   .Add("\"" + _inputFile + "\"");
        });
    }
}

/// <summary>
/// Represents the export options used when converting a document to PDF format using LibreOffice.
/// </summary>
public class ExportOptions
{
    /// <summary>
    /// Gets or sets the path to the user installation directory.
    /// </summary>
    public string UserInstallation { get; set; }

    /// <summary>
    /// Gets or sets the path to the output directory where the PDF file will be saved.
    /// </summary>
    public string OutputDirectory { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether LibreOffice should be started in headless mode (i.e., without a user interface).
    /// </summary>
    public bool Headless { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether LibreOffice should be started in invisible mode (i.e., without a window).
    /// </summary>
    public bool Invisible { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the LibreOffice logo should be suppressed during startup.
    /// </summary>
    public bool NoLogo { get; set; }

    /// <summary>
    /// Gets or sets the name of the printer to be used for printing.
    /// </summary>
    public string PrinterName { get; set; }

    /// <summary>
    /// Gets or sets the IP address or hostname of the printer to be used for printing.
    /// </summary>
    public string PrinterAddress { get; set; }

    /// <summary>
    /// Gets or sets the port number of the printer to be used for printing.
    /// </summary>
    public int? PrinterPort { get; set; }

    /// <summary>
    /// Gets or sets the name of the printer driver to be used for printing.
    /// </summary>
    public string PrinterDriver { get; set; }

    /// <summary>
    /// Gets or sets the type of the printer to be used for printing (e.g., "ps", "pdf", "jpeg").
    /// </summary>
    public string PrinterType { get; set; }

    /// <summary>
    /// Gets or sets the resolution of the printer to be used for printing (e.g., "300", "600", "1200").
    /// </summary>
    public int? PrinterResolution { get; set; }

    /// <summary>
    /// Gets or sets the paper size to be used for printing (e.g., "A4", "Letter").
    /// </summary>
    public string PrinterPaperSize { get; set; }

    /// <summary>
    /// Gets or sets the paper source to be used for printing (e.g., "auto", "tray1", "manual").
    /// </summary>
    public string PrinterPaperSource { get; set; }

    /// <summary>
    /// Gets or sets the paper orientation to be used for printing (e.g., "portrait", "landscape").
    /// </summary>
    public string PrinterPaperOrientation { get; set; }

    /// <summary>
    /// Gets or sets the duplex mode to be used for printing (e.g., "simplex", "long-edge", "short-edge").
    /// </summary>
    public string PrinterDuplex { get; set; }

    /// <summary>
    /// Gets or sets the number of copies to be printed.
    /// </summary>
    public int? PrinterCopies { get; set; }

    /// <summary>
    /// Gets or/// sets a value indicating whether collation of printed copies should be enabled.
    /// </summary>
    public bool PrinterCollate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether reverse order printing should be enabled.
    /// </summary>
    public bool PrinterReverse { get; set; }

    /// <summary>
    /// Gets or sets the name of the print job.
    /// </summary>
    public string PrinterJobName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user should be notified when the print job is complete.
    /// </summary>
    public bool PrinterNotify { get; set; }

    /// <summary>
    /// Gets or sets the timeout for the print job, in seconds.
    /// </summary>
    public int? PrinterTimeout { get; set; }
}