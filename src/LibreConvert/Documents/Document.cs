
namespace LibreConvert.Documents;

public abstract class Document
{
    /// <summary>
    ///     Exports the document to a PDF file
    /// </summary>
    /// <param name="outputDirectory"></param>
    /// <returns></returns>
    public abstract Task ExportAsync(string outputDirectory);
}